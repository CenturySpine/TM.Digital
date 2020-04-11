using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Services.Common;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Services
{

    public static class ModelFactory
    {

        public static Player NewPlayer(string name, bool test)
        {
            return new Player
            {
                IsReady = false,
                TotalActions = 2,
                RemainingActions = 0,
                PlayerId = Guid.NewGuid(),
                Name = name,
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
                Corporation = new Corporation(),
                TerraformationLevel = 20,

            };
        }
    }

    internal class GameSession
    {
        private class PlayersTracking
        {
            private HashSet<Guid> _initialOrder = new HashSet<Guid>();

            public Guid AddOrderedPlayers(HashSet<Guid> order)
            {
                _initialOrder = new HashSet<Guid>(order);
                _remaining = new HashSet<Guid>(_initialOrder);
                return _remaining.First();
            }

            private HashSet<Guid> _remaining = new HashSet<Guid>();

            private Queue<Guid> _playingQueue;

            internal async Task<bool> NewTurn()
            {
                await Logger.Log("na", $"Trying to start new turn...");
                _playingQueue = new Queue<Guid>(_remaining);
                if (_playingQueue.Count == 0)
                {
                    await Logger.Log("na", $"Playing queue is empty, GENERATION IS OVER");
                    return true;//end of generation, every one passed
                }

                return false;
            }

            public async Task<Guid> PickNextPlayer()
            {
                await Logger.Log("na", $"Picking next player...");
                if (_playingQueue.Count == 0)
                {
                    await Logger.Log("na", $"Queue is empty...");
                    var nextGeneration = await NewTurn();
                    if (nextGeneration)
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

            public void NewGeneration()
            {
                var previousturnorder = _initialOrder.ToList();
                var first = previousturnorder.First();
                previousturnorder.Remove(first);

                previousturnorder.Insert(previousturnorder.Count > 1 ? previousturnorder.Count - 1 : 0, first);

                _initialOrder = previousturnorder.ToHashSet();

                _remaining = new HashSet<Guid>(_initialOrder);
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
            await Logger.Log("na", "Initializing...");
            _cardDrawer = new CardDrawer();
            _playerRandomization = new Random((int)(DateTime.Now.Millisecond / 3.5));

            Board = BoardGenerator.Instance.Original();
            PlayerTack = new PlayersTracking();
        }

        private PlayersTracking PlayerTack { get; set; }

        private async Task RandomizePlayers()
        {
            await Logger.Log("na", "Randomizing players...");

            HashSet<Guid> ordered = new HashSet<Guid>();
            while (ordered.Count != Players.Count)
            {
                var nextPlayerIndex = _playerRandomization.Next(0, Players.Count);

                var play = Players.Select(p => p.Key).ToList()[nextPlayerIndex];
                if (ordered.Add(play))
                {
                    await Logger.Log("na", $"Player {Players[play].Name} added {play}");
                }
            }
            PlayerTack.AddOrderedPlayers(ordered);
        }

        public Player AddPlayer(string playerName, bool test)
        {
            var player = ModelFactory.NewPlayer(playerName, test);
            Players.Add(player.PlayerId, player);

            return player;
        }

        public async Task PlaceTile(BoardPlace place, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (PendingTileEffect != null)
            {
                if (Players.TryGetValue(playerId, out var player))
                {
                    await Logger.Log(player.Name, $"Placing tile '{PendingTileEffect.Type}' on place '{place.Index}'");
                    await BoardHandler.PlaceTileOnBoard(place, player, PendingTileEffect, Board, _cardDrawer);
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
                var choices = await CardPlayHandler.Play(card, player, Board);
                if (choices != null)
                {
                    if (choices.TileEffects != null && choices.TileEffects.Any())
                    {
                        await Logger.Log(player.Name, $"Playing card '{card.Name}' requires player '{player.Name}' to make {choices.TileEffects.Count} tile choices");
                        foreach (var choicesTileEffect in choices.TileEffects)
                        {
                            RemainingActions.Enqueue(async (p, b) =>
                                {
                                    PendingTileEffect = choicesTileEffect;
                                    await Logger.Log(player.Name, $"Effect {PendingTileEffect.Type}... Getting board available spaces...");

                                    var choiceBoard = BoardHandler.GetPlacesChoices(PendingTileEffect, b);
                                    await Logger.Log(player.Name, $"Found {choiceBoard.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => p.CanBeChosed).Count()} available places. Sending choices to player");

                                    await hubContext.Clients.All.SendAsync(ServerPushMethods.PlaceTileRequest, $"{p.PlayerId}", JsonSerializer.Serialize(choiceBoard));
                                });
                        }
                    }
                    RemainingActions.Enqueue(async (p, b) =>
                    {
                        await EffectHandler.CheckCardsReductions(p);
                        await VerifyRemainingAction(p, hubContext);
                    });
                    RemainingActions.Enqueue(async (p, b) =>
                    {
                        await PrerequisiteHandler.CanPlayCards(b, p);
                        await VerifyRemainingAction(p, hubContext);
                    });
                    RemainingActions.Enqueue(async (p, b) =>
                    {
                        p.RemainingActions--;
                        if (p.RemainingActions == 0)
                        {
                            await Logger.Log(p.Name, $"No remaining action, auto skip");
                            //await UpdateGame(hubContext);
                            await Skip(p.PlayerId, hubContext);
                        }
                        else
                        {
                            await VerifyRemainingAction(p, hubContext);
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
            }


        }

        public async Task<Player> SetupPlayer(GameSetupSelection selection, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(selection.PlayerId, out var player))
            {
                if (selection.Corporation != null)
                {
                    await Logger.Log(player.Name, $"Receiving player setup. chosen corporation = '{selection.Corporation.Name}'");
                    player.Corporation = selection.Corporation;
                    foreach (var corporationEffect in selection.Corporation.ResourcesEffects)
                    {
                        await EffectHandler.HandleResourceEffect(player, corporationEffect);
                    }
                }


                await Logger.Log(player.Name, $"Patent bought : {selection.BoughtCards.Count}");

                await EffectHandler.HandleInitialPatentBuy(player, selection.BoughtCards, selection.Corporation);

                

                await EffectHandler.CheckCardsReductions(player);
                await PrerequisiteHandler.CanPlayCards(Board, player);

                player.IsReady = true;
                await Logger.Log(player.Name, $"Ready");
                if (Players.All(p => p.Value.IsReady))
                {
                    await Logger.Log("na", $"All players ready... launching game");
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
                await Logger.Log(player.Name, $"Skipping !");
                var playing = await PlayerTack.PickNextPlayer();
                if (playing != Guid.Empty)
                {
                    player.RemainingActions = 0;
                    await SendPleyrsPlaying(hubContext, playing);
                }
                else
                {
                    //TODO next generation
                    await Logger.Log("na", $"GENERATION IS OVER...");
                    await StartNewGeneration(hubContext);
                }
                return true;
            }

            return false;
        }

        private async Task StartNewGeneration(IHubContext<ClientNotificationHub> hubContext)
        {
            Board.Generation++;
            foreach (var player in Players)
            {
                foreach (var resourceHandler in player.Value.Resources)
                {
                    if (resourceHandler.ResourceType == ResourceType.Money)
                    {
                        resourceHandler.UnitCount += (resourceHandler.Production + player.Value.TerraformationLevel);
                    }
                    else
                    {
                        resourceHandler.UnitCount += resourceHandler.Production;
                    }
                }
            }

            PlayerTack.NewGeneration();
            await SendSetup(hubContext, false);
            await UpdateGame(hubContext);
        }

        private async Task SendPleyrsPlaying(IHubContext<ClientNotificationHub> hubContext, Guid playing)
        {
            if (Players.TryGetValue(playing, out var nextPlayer))
            {
                await Logger.Log(nextPlayer.Name, $"Turn to play !");
                nextPlayer.RemainingActions = 2;
                await UpdateGame(hubContext);
                await hubContext.Clients.All.SendAsync(ServerPushMethods.Playing, playing, nextPlayer.Name);
            }
        }

        public async Task<bool> Pass(Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                await Logger.Log(player.Name, $"Passing !");
                player.RemainingActions = 0;
                PlayerTack.PlayerPassed(playerId);
                await Skip(playerId, hubContext);
            }

            return false;
        }

        private async Task Launch(IHubContext<ClientNotificationHub> hubContext)
        {
            if (!await PlayerTack.NewTurn())
            {
                var playing = await PlayerTack.PickNextPlayer();

                await SendPleyrsPlaying(hubContext, playing);
            }
        }

        public async Task<bool> Start(IHubContext<ClientNotificationHub> hubContext)
        {
            try
            {
                await Logger.Log("na", "Starting game");
                await SendSetup(hubContext, true);
                await RandomizePlayers();
            }
            catch (Exception e)
            {
                await Logger.Log("ERROR", e.ToString());
                return false;
            }
            return true;
        }

        private async Task SendSetup(IHubContext<ClientNotificationHub> hubContext, bool isInitialSetup)
        {
            foreach (var player in Players)
            {
                var setup = CreatePlayerSetup(player.Key, isInitialSetup);
                await Logger.Log(player.Value.Name, "Sending game setup...");
                await hubContext.Clients.All.SendAsync(ServerPushMethods.SetupChoice, player.Key,
                    JsonSerializer.Serialize(setup));
            }
        }

        private GameSetup CreatePlayerSetup(Guid player, bool isInitialSetup)
        {
            if (Players.TryGetValue(player, out var playerObj))
            {
                GameSetup gs = new GameSetup
                {
                    PlayerId = playerObj.PlayerId,
                    Corporations = new List<Corporation>(),
                    Patents = new List<Patent>(),
                    GameId = this.Id,
                    IsInitialSetup = isInitialSetup
                };

                if (isInitialSetup)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        gs.Corporations.Add(_cardDrawer.DrawCorporation());
                    }
                }

                for (int i = 0; i < (isInitialSetup ? 4 : 2); i++)
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
                await Logger.Log(player.Name, $"All actions choices done. Sending game update to all players");

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