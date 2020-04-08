using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Services
{
    internal class GameSession
    {
        private class PlayersTracking
        {
            private readonly HashSet<Guid> _initialOrder = new HashSet<Guid>();

            public Guid AddOrderedPlayer(Guid to)
            {
                _initialOrder.Add(to);
                _remaining = new HashSet<Guid>(_initialOrder);
                return to;
            }

            private HashSet<Guid> _remaining = new HashSet<Guid>();

            private Queue<Guid> _playingQueue;

            internal bool NewTurn()
            {
                _playingQueue = new Queue<Guid>(_remaining);
                if (_playingQueue.Count == 0)
                {
                    return true;//end of generation, every one passed
                }

                return false;
            }

            public Guid PickNextPlayer()
            {
                if (_playingQueue.Count == 0)
                {
                    var next = NewTurn();
                    if (!next)
                    {
                        return Guid.Empty;
                    }
                }
                return _playingQueue.Dequeue();
            }

            public void PlayerPassed(Guid player)
            {
                _remaining.Remove(player);
            }
        }

        private static TileEffect PendingTileEffect;
        private static Queue<Action<Player, Board>> RemainingActions = new Queue<Action<Player, Board>>();
        private CardDrawer _cardDrawer;
        private Random _playerRandomization;

        private Board Board { get; set; }
        public Guid Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public Dictionary<Guid, Player> Players { get; set; }

        internal async Task Initialize()
        {
            await Task.CompletedTask;
            _cardDrawer = new CardDrawer();
            _playerRandomization = new Random((int)(DateTime.Now.Millisecond / 3.5));

            Board = BoardGenerator.Instance.Original();
            PlayerTack = new PlayersTracking();
        }

        private PlayersTracking PlayerTack { get; set; }

        private void RandomizePlayers()
        {
            int seed = Players.Count;

            while (seed >= 0)
            {
                var nextPlayerIndex = _playerRandomization.Next(seed);
                seed--;
                var picked=PlayerTack.AddOrderedPlayer(Players.Select(p => p.Key).ToList()[nextPlayerIndex]);
                
            }
        }

        public Player AddPlayer(string playerName, bool test)
        {
            var player = new Player
            {
                IsReady = false,
                TotalActions = 2,
                RemainingActions = 0,
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

            return player;
        }

        public async Task PlaceTile(BoardPlace place, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (PendingTileEffect != null)
            {
                if (Players.TryGetValue(playerId, out var player))
                {
                    Logger.Log(player.Name, $"Placing tile '{PendingTileEffect.Type}' on place '{place.Index}'");
                    BoardHandler.PlaceTileOnBoard(place, player, PendingTileEffect, Board, _cardDrawer);
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

                                    await hubContext.Clients.All.SendAsync(ServerPushMethods.PlaceTileRequest, $"{p.PlayerId}", JsonSerializer.Serialize(choiceBoard));
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
                    RemainingActions.Enqueue(async (p, b) =>
                    {
                        p.RemainingActions--;
                        if (p.RemainingActions == 0)
                        {
                            await Skip(p.PlayerId, hubContext);
                        }
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

        public async Task<Player> SetupPlayer(GameSetupSelection selection, IHubContext<ClientNotificationHub> hubContext)
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

                player.IsReady = true;
                if (Players.All(p => p.Value.IsReady))
                {
                    await Launch(hubContext);
                }
                return player;
            }
            return null;
        }
        public async Task<bool> Skip(Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var playing = PlayerTack.PickNextPlayer();
                if (playing != Guid.Empty)
                {
                    player.RemainingActions = 0;
                    if (Players.TryGetValue(playing, out var nextPlayer))
                    {
                        nextPlayer.RemainingActions = 2;
                        await UpdateGame(hubContext);
                        await hubContext.Clients.All.SendAsync(ServerPushMethods.Playing, playing, string.Empty);
                    }
                }
                else
                {
                    //TODO next generation
                }
                return true;
            }

            return false;
        }

        public async Task<bool> Pass(Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                player.RemainingActions = 0;
                PlayerTack.PlayerPassed(playerId);
                await Skip(playerId, hubContext);

            }

            return false;
        }
        private async Task Launch(IHubContext<ClientNotificationHub> hubContext)
        {
            if (!PlayerTack.NewTurn())
            {
                var playing = PlayerTack.PickNextPlayer();
                await UpdateGame(hubContext);
                await hubContext.Clients.All.SendAsync(ServerPushMethods.Playing, playing, string.Empty);
            }
        }

        public async Task<bool> Start(IHubContext<ClientNotificationHub> hubContext)
        {
            try
            {
                foreach (var player in Players)
                {
                    var setup = CreatePlayerSetup(player.Value.Name);
                    await hubContext.Clients.All.SendAsync(ServerPushMethods.SetupChoice, player.Key, JsonSerializer.Serialize(setup));
                    RandomizePlayers();
                }
            }
            catch (Exception e)
            {
                Logger.Log("ERROR", e.ToString());
                return false;
            }
            return true;
        }

        private GameSetup CreatePlayerSetup(string player)
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
                    gs.Corporations.Add(_cardDrawer.DrawCorporation());
                }

                for (int i = 0; i < 4; i++)
                {
                    gs.Patents.Add(_cardDrawer.DrawPatent());
                }

                return gs;
            }

            return null;
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

                await UpdateGame(hubContext);
            }
        }

        private async Task UpdateGame(IHubContext<ClientNotificationHub> hubContext)
        {
            var game = new Game
            {
                Board = Board,
                AllPlayers = Players.Select(p => p.Value).ToList(),
            };
            await hubContext.Clients.All.SendAsync(ServerPushMethods.RecieveGameUpdate, "PlayResult",
                JsonSerializer.Serialize(game));
        }
    }
}