using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Services
{
    internal class GameSession
    {
        public string Owner { get; set; }
        public Guid Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Queue<Corporation> AvailableCorporations { get; set; }
        public Queue<Patent> AvailablePatents { get; set; }
        public Dictionary<Guid, Player> Players { get; set; }

        public void AddPlayer(string playerName, bool test)
        {
            var player = new Player
            {
                PlayerId = Guid.NewGuid(),
                Name = playerName,
                Resources = new List<ResourceHandler>
                {
                    new ResourceHandler {ResourceType = ResourceType.Money,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Steel,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Titanium,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Plant,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Energy,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Heat,UnitCount = test?20:0,Production = test?5:0}
                },
                HandCards = new List<Patent>(),
                PlayedCards = new List<Patent>(),
                TerraformationLevel = 20,
            };
            Players.Add(player.PlayerId, player);

            //return CreatePlayerSetup(player);
        }

        public GameSetup CreatePlayerSetup(string player)
        {
            var playerObj = Players.FirstOrDefault(p => p.Value.Name.Equals(player));

            if (playerObj.Value != null)
            {


                GameSetup gs = new GameSetup
                {
                    PlayerId = playerObj.Value.PlayerId,
                    Corporations = new List<Corporation>(),
                    Patents = new List<Patent>(),
                    GameId = this.Id
                };


                for (int i = 0; i < 2; i++)
                {
                    gs.Corporations.Add(AvailableCorporations.Dequeue());
                }

                while (AvailablePatents.Count > 0)
                {
                    gs.Patents.Add(AvailablePatents.Dequeue());
                }


                return gs;

            }

            return null;
        }

        public Player SetupPlayer(GameSetupSelection selection)
        {
            if (Players.TryGetValue(selection.PlayerId, out var player))
            {
                foreach (var corporationEffect in selection.Corporation.ResourcesEffects)
                {
                    EffectHandler.HandleResourceEffect(player, corporationEffect);
                }

                EffectHandler.HandleInitialPatentBuy(player, selection.BoughtCards, selection.Corporation);
                player.Corporation = selection.Corporation;

                EffectHandler.CheckCardsReductions(player);
                PrerequisiteHandler.CanPlayCards(Board, player);

                return player;
            }
            return null;
        }
        static Queue<Action<Player, Board>> RemainingActions = new Queue<Action<Player, Board>>();
        private static TileEffect PendingTileEffect;
        public async Task PlayCard(Patent card, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var choices = CardPlayHandler.Play(card, player, Board);
                if (choices != null)
                {


                    if (choices.TileEffects != null && choices.TileEffects.Any())
                    {
                        Logger.Log(player.Name, $"Playing card '{card.Name}' requires player '{player.Name}' to make {choices.TileEffects.Count} tile choices");
                        foreach (var choicesTileEffect in choices.TileEffects)
                        {
                            RemainingActions.Enqueue(async (p, b) =>
                                {
                                    PendingTileEffect = choicesTileEffect;
                                    Logger.Log(player.Name, $"Effect {PendingTileEffect.Type}... Getting board available spaces...");

                                    var choiceBoard = BoardHandler.GetPlacesChoices(PendingTileEffect, b);
                                    Logger.Log(player.Name, $"Found {choiceBoard.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => p.CanBeChosed).Count()} available places. Sending choices to player");
                                    await hubContext.Clients.All.SendAsync("PlaceTile", $"{p.PlayerId}", JsonSerializer.Serialize(choiceBoard));

                                });
                        }

                    }
                    RemainingActions.Enqueue(async (p, b) =>
                    {

                        EffectHandler.CheckCardsReductions(p);
                        await VerifyRemainingAction(p, hubContext);
                    });
                    RemainingActions.Enqueue(async (p, b) =>
                    {
                        PrerequisiteHandler.CanPlayCards(b, p);
                        await VerifyRemainingAction(p, hubContext);
                    });
                }

                if (RemainingActions.Any())
                {
                    await Task.Run(() =>
                    {
                        var action = RemainingActions.Dequeue();
                        action.Invoke(player, Board);
                    });

                }
                //EffectHandler.CheckCardsReductions(player);
                //PrerequisiteHandler.CanPlayCards(Board, player);
            }

            //await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", "PlayResult", JsonSerializer.Serialize(playResult));


            //return new Game
            //{
            //    Board = Board,
            //    AllPlayers = Players.Select(p => p.Value).ToList(),
            //};
        }

        private async Task VerifyRemainingAction(Player player, IHubContext<ClientNotificationHub> hubContext)
        {

            if (RemainingActions.Any())
            {
                var action = RemainingActions.Dequeue();
                action.Invoke(player, Board);
            }
            else
            {
                Logger.Log(player.Name, $"All actions choices done. Sending game update to all players");

                var game = new Game
                {
                    Board = Board,
                    AllPlayers = Players.Select(p => p.Value).ToList(),
                };
                await hubContext.Clients.All.SendAsync("ReceiveGameUpdate", "PlayResult", JsonSerializer.Serialize(game));



            }
        }

        public async Task PlaceTile(BoardPlace place, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (PendingTileEffect != null)
            {

                if (Players.TryGetValue(playerId, out var player))
                {
                    Logger.Log(player.Name, $"Placing tile '{PendingTileEffect.Type}' on place '{place.Index}'");
                    BoardHandler.PlaceTileOnBoard(place, player, PendingTileEffect, Board, AvailablePatents);
                    PendingTileEffect = null;
                    await VerifyRemainingAction(player, hubContext);
                }
                //TODO manage
            }
            else
            {
                //TODO manage
            }
        }
    }
}