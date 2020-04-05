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
        public Guid Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Queue<Corporation> AvailableCorporations { get; set; }
        public Queue<Patent> AvailablePatents { get; set; }
        public Dictionary<Guid, Player> Players { get; set; }

        public GameSetup AddPlayer(string playerName)
        {
            var player = new Player
            {
                PlayerId = Guid.NewGuid(),
                Name = playerName,
                Resources = new List<ResourceHandler>
                {
                    new ResourceHandler {ResourceType = ResourceType.Money},
                    new ResourceHandler {ResourceType = ResourceType.Steel},
                    new ResourceHandler {ResourceType = ResourceType.Titanium},
                    new ResourceHandler {ResourceType = ResourceType.Plant},
                    new ResourceHandler {ResourceType = ResourceType.Energy},
                    new ResourceHandler {ResourceType = ResourceType.Heat}
                },
                HandCards = new List<Patent>(),
                PlayedCards = new List<Patent>(),
                TerraformationLevel = 20,
            };
            Players.Add(player.PlayerId, player);
            GameSetup gs = new GameSetup
            {
                PlayerId = player.PlayerId,
                Corporations = new List<Corporation>(),
                Patents = new List<Patent>(),
                GameId = this.Id
            };
            for (int i = 0; i < 2; i++)
            {
                gs.Corporations.Add(AvailableCorporations.Dequeue());
            }
            for (int i = 0; i < 4; i++)
            {
                gs.Patents.Add(AvailablePatents.Dequeue());
            }

            return gs;
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
                        foreach (var choicesTileEffect in choices.TileEffects)
                        {
                            RemainingActions.Enqueue(async (p, b) =>
                                {
                                    PendingTileEffect = choicesTileEffect;
                                    var choiceBoard = BoardHandler.GetPlacesChoices(PendingTileEffect, b);
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