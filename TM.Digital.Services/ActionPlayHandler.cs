using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Services.Common;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Services
{
    public static class ActionPlayHandler
    {
        public static async Task<List<Action<Player, Board>>> Convert(Player playerObj, ResourceHandler resources,
            Board board, CardDrawer cardDrawer)
        {
            await Task.CompletedTask;
            List<Action<Player, Board>> currentActions = new List<Action<Player, Board>>();

            switch (resources.ResourceType)
            {
                case ResourceType.Plant:
                    var convPlants = PrerequisiteHandler.GetPlantConversionRate(playerObj);
                    playerObj[ResourceType.Plant].UnitCount -= convPlants;
                    var teffect = new TileEffect
                    {
                        Type = TileType.Forest,
                        Constrains = TilePlacementCosntrains.StandardForest,
                        Number = 1
                    };
                    currentActions.Add(TileEffectAction(playerObj, teffect, board));
                    break;
                case ResourceType.Heat:

                    //gt conversion rate
                    var conv = PrerequisiteHandler.GetHeatConversionRate(playerObj);

                    //withdraw corresponding heat units
                    playerObj[ResourceType.Heat].UnitCount -= conv;

                    //increase global parameter
                    await BoardEffectHandler.IncreaseParameterLevel(board, BoardLevelType.Temperature, playerObj, 1, cardDrawer);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return currentActions;
        }
        public static async Task<List<Action<Player, Board>>> Play(CardActionPlay cardAction, Player player, Board board,
            List<Player> allPlayers, CardDrawer cardDrawer)

        {
            var card = cardAction.Patent;

            List<Action<Player, Board>> currentActions = new List<Action<Player, Board>>();

            await Logger.Log(player.Name, $"Player '{player.Name}' playing card '{card.Name}'");

            //Reduce cost based on mineral units played
            var cost = card.ModifiedCost;
            //int mineralValue = 0;

            //for each mineral usage sent by player
            foreach (var actionPlayResourcesUsage in cardAction.ResourcesUsages)
            {
                //calculate mineral value
                //mineralValue +=
                //    player[actionPlayResourcesUsage.ResourceType].MoneyValueModifier *//based on mineral value modifier
                //                actionPlayResourcesUsage.UnitPlayed; //multiplied by unit played

                //remove resources unit played from players resources.
                player[actionPlayResourcesUsage.ResourceType].UnitCount -= actionPlayResourcesUsage.UnitPlayed;
            }

            //calculate final patent cost
            //cost = cost - mineralValue;
            //ensure it never goes below 0;
            if (cost < 0) cost = 0;

            //reduce money unit by the amount of the final modified patent cost
            player[ResourceType.Money].UnitCount -= cost;

            player.HandCards.Remove(card);
            player.PlayedCards.Add(card);

            //resources effects for self
            foreach (var cardResourceEffect in card.ResourcesEffects/*.Where(re => re.EffectDestination == ActionTarget.Self)*/)
            {
                Action<Player, Board> action =
                await EffectHandler.HandleResourceEffect(player, cardResourceEffect, allPlayers, board, cardDrawer, card);
                if (action != null)
                {
                    currentActions.Add(action);
                }
            }

            ////resources effect for others
            //foreach (var cardResourceEffect in card.ResourcesEffects.Where(re => re.EffectDestination == ActionTarget.AnyPlayer))
            //{

            //    currentActions.Add(ResourceEffectActionChoice(cardResourceEffect, allPlayers));
            //}

            foreach (var boardLevelEffect in card.BoardEffects)
            {
                await BoardEffectHandler.HandleBoardEffect(boardLevelEffect, board, player, cardDrawer);
            }

            if (card.TileEffects.Any())
            {
                await Logger.Log(player.Name, $"Playing card '{card.Name}' requires player '{player.Name}' to make {card.TileEffects.Count} tile choices");
                foreach (var choicesTileEffect in card.TileEffects)
                {
                    currentActions.Add(TileEffectAction(player, choicesTileEffect, board));
                }
            }

            return currentActions;
        }



        private static Action<Player, Board> TileEffectAction(Player player, TileEffect choicesTileEffect, Board board)
        {
            //await async (p, b) =>
            // {
            return new Action<Player, Board>(async (p, b) =>
            {

                //special cases for oceans
                if (choicesTileEffect.Type == TileType.Ocean)
                {
                    if (BoardEffectHandler.HasReachedMaxParam(board, BoardLevelType.Oceans))
                    {
                        return;
                    }

                }
                BoardTilesHandler.PendingTileEffect = choicesTileEffect;
                await Logger.Log(player.Name,
                    $"Effect {BoardTilesHandler.PendingTileEffect.Type}... Getting board available spaces...");

                var choiceBoard = BoardTilesHandler.GetPlacesChoices(BoardTilesHandler.PendingTileEffect, b, player);
                if (choiceBoard == null) // no more space giving constraints
                {
                    return;
                }

                await Logger.Log(player.Name,
                    $"Found {choiceBoard.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => p.CanBeChoosed).Count()} available places. Sending choices to player");

                await Hubconcentrator.Hub.Clients.All.SendAsync(ServerPushMethods.PlaceTileRequest,
                    $"{p.PlayerId}", JsonSerializer.Serialize(choiceBoard));
            });

            //};
        }


        public static async Task<List<Action<Player, Board>>> ExecuteBoardAction(BoardAction boardAction, Board board, Player player)
        {
            await Task.CompletedTask;
            switch (boardAction.BoardActionType)
            {
                case BoardActionType.PatentsSell:
                    break;
                case BoardActionType.PowerPlant:
                    return await PlayAction(boardAction.Actions, player, board);
                case BoardActionType.Asteroid:
                    return await PlayAction(boardAction.Actions, player, board);
                case BoardActionType.Aquifere:
                    return await PlayAction(boardAction.Actions, player, board);
                case BoardActionType.Forest:
                    return await PlayAction(boardAction.Actions, player, board);
                case BoardActionType.City:
                    return await PlayAction(boardAction.Actions, player, board);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new List<Action<Player, Board>>();
        }
        public static async Task<IEnumerable<Action<Player, Board>>> ExecuteAction(Action action, Board board, Player player, CardDrawer cardDrawer)
        {
            var choices = await PlayAction(action, player, board, cardDrawer);

            var targetCard = player.PlayedCards.FirstOrDefault(c => c.Guid == action.CardId);
            if (targetCard != null)
            {
                targetCard.ActionPlayed = true;
            }

            return choices;
        }
        public static async Task<List<Action<Player, Board>>> PlayAction(Action action, Player player, Board board, CardDrawer cardDrawer = null)
        {
            await Task.CompletedTask;
            List<Action<Player, Board>> currentActions = new List<Action<Player, Board>>();

            if (action.ActionFrom != null)
            {
                if (action.ActionFrom.ActionTarget == ActionTarget.Self)
                {
                    if (!action.ActionFrom.ResourceEffectsAlternatives.Any())
                    {
                        return new List<Action<Player, Board>>();//TODO manage
                    }
                    //spend resources
                    await EffectHandler.HandleResourceEffect(player, action.ActionFrom.ResourceEffect, new List<Player>(), board,
                        cardDrawer, null);

                    //var resourceCost = player[action.ActionFrom.ResourceEffect.ResourceType];
                    //switch (action.ActionFrom.ResourceEffect.ResourceKind)
                    //{
                    //    case ResourceKind.Unit:
                    //        resourceCost.UnitCount += action.ActionFrom.ResourceEffect.Amount;
                    //        break;
                    //    case ResourceKind.Production:
                    //        resourceCost.Production += action.ActionFrom.ResourceEffect.Amount;
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}

                    foreach (var actionTo in action.ActionTo)
                    {
                        //if (actionTo.ActionTarget == ActionTarget.Self)
                        //{
                        //get action benefits

                        if (actionTo.ResourceEffect != null)
                            await EffectHandler.HandleResourceEffect(player, actionTo.ResourceEffect, new List<Player>(), board,
                                cardDrawer, null);

                        //for resources
                        //if (actionTo.ResourceEffect != null && actionTo.ResourceEffect.ResourceType == ResourceType.Card && cardDrawer != null)
                        //{
                        //    for (int i = 0; i < actionTo.ResourceEffect.Amount; i++)
                        //    {
                        //        player.HandCards.Add(cardDrawer.DrawPatent());
                        //    }

                        //}
                        //else if (actionTo.ResourceEffect != null)
                        //{
                        //    var resourceBenefit = player[actionTo.ResourceEffect.ResourceType];
                        //    switch (actionTo.ResourceEffect.ResourceKind)
                        //    {
                        //        case ResourceKind.Unit:
                        //            resourceBenefit.UnitCount += actionTo.ResourceEffect.Amount;
                        //            break;
                        //        case ResourceKind.Production:
                        //            resourceBenefit.Production += actionTo.ResourceEffect.Amount;
                        //            break;
                        //        default:
                        //            throw new ArgumentOutOfRangeException();
                        //    }
                        //}

                        //for tile
                        if (actionTo.TileEffect != null)
                        {
                            currentActions.Add(TileEffectAction(player, actionTo.TileEffect, board));
                        }

                        //for global parameter
                        if (actionTo.BoardLevelEffect != null)
                        {
                            await BoardEffectHandler.IncreaseParameterLevel(board, actionTo.BoardLevelEffect.BoardLevelType, player, actionTo.BoardLevelEffect.Level, cardDrawer);

                            //var parameter = board.Parameters.FirstOrDefault(p => p.Type == actionTo.BoardLevelType);
                            //if (parameter != null)
                            //{
                            //    var value = actionTo.Amount * parameter.GlobalParameterLevel.Increment;


                            //    parameter.GlobalParameterLevel.Level += value;
                            //    player.TerraformationLevel += actionTo.Amount;


                            //}
                        }
                        //}
                    }
                }
            }

            return currentActions;
        }


    }
}