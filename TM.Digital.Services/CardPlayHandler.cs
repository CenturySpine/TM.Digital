using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Services.Common;

namespace TM.Digital.Services
{
    public static class CardPlayHandler
    {
        public static async Task<List<Action<Player, Board>>> Play(Patent card, Player player, Board board,
            List<Player> allPlayers)
        {
            List<Action<Player, Board>> currentactions = new List<Action<Player, Board>>();
            await Logger.Log(player.Name, $"Player '{player.Name}' playing card '{card.Name}'");

            player.Resources.First(r => r.ResourceType == ResourceType.Money).UnitCount -= card.ModifiedCost;
            //Choices ch = new Choices();
            player.HandCards.Remove(card);
            player.PlayedCards.Add(card);

            //resources effects for self
            foreach (var cardResourceEffect in card.ResourcesEffects.Where(re => re.EffectDestination == EffectDestination.Self))
            {
                await EffectHandler.HandleResourceEffect(player, cardResourceEffect);
            }

            //resources effect for others
            foreach (var cardResourceEffect in card.ResourcesEffects.Where(re => re.EffectDestination == EffectDestination.OtherPlayer))
            {
                currentactions.Add(ResourceEffectActionChoice(cardResourceEffect, allPlayers));
                //await EffectHandler.HandleResourceEffect(player, cardResourceEffect);
            }

            foreach (var boardLevelEffect in card.BoardEffects)
            {
                await BoardEffectHandler.HandleBoardEffect(boardLevelEffect, board, player);
            }

            if (card.TileEffects.Any())
            {
                await Logger.Log(player.Name, $"Playing card '{card.Name}' requires player '{player.Name}' to make {card.TileEffects.Count} tile choices");
                foreach (var choicesTileEffect in card.TileEffects)
                {
                    currentactions.Add(TileEffectAction(player, choicesTileEffect, board));

                }
            }

            return currentactions;
        }

        private static Action<Player, Board> ResourceEffectActionChoice(ResourceEffect cardResourceEffect, List<Player> allPlayers)
        {
            return new Action<Player, Board>(async (p, b) =>
            {
                ResourceEffectPlayerChooserList chooser = new ResourceEffectPlayerChooserList()
                {
                    ChoicesList = allPlayers.Select(target =>
                    {
                        var targetPlayerREsource = target.Resources
                            .First(r => r.ResourceType == cardResourceEffect.ResourceType);
                        return new ResourceEffectPlayerChooser
                        {
                            TargetPlayerId = target.PlayerId,
                            TargetPlayerName = target.Name,
                            ResourceHandler = cardResourceEffect,
                            
                        };
                    }).ToList()
                    
                };
                chooser.ChoicesList.Add(new ResourceEffectPlayerChooser()
                {
                    TargetPlayerId = GameSession._neutralPlayerId,
                    TargetPlayerName = "Neutral Player / Nobody",
                    ResourceHandler = cardResourceEffect,
                });

                await Hubconcentrator.Hub.Clients.All.SendAsync(ServerPushMethods.ResourceEffectForOtherPlayer,
                    $"{p.PlayerId}", JsonSerializer.Serialize(chooser));
            });
        }

        private static Action<Player, Board> TileEffectAction(Player player, TileEffect choicesTileEffect, Board b)
        {
            //await async (p, b) =>
            // {

            return new Action<Player, Board>(async (p, b) =>
            {
                BoardHandler.PendingTileEffect = choicesTileEffect;
                await Logger.Log(player.Name,
                    $"Effect {BoardHandler.PendingTileEffect.Type}... Getting board available spaces...");

                var choiceBoard = BoardHandler.GetPlacesChoices(BoardHandler.PendingTileEffect, b);
                await Logger.Log(player.Name,
                    $"Found {choiceBoard.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => p.CanBeChosed).Count()} available places. Sending choices to player");

                await Hubconcentrator.Hub.Clients.All.SendAsync(ServerPushMethods.PlaceTileRequest,
                    $"{p.PlayerId}", JsonSerializer.Serialize(choiceBoard));
            });

            //};
        }
    }
}