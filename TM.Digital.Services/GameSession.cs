﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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
using TM.Digital.Services.Common;
using TM.Digital.Transport.Hubs.Hubs;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Services
{
    internal class GameSession
    {
        internal static readonly Guid _neutralPlayerId = Guid.Parse("{C47F5A45-601D-4ECA-B555-7D0F2AF83062}");
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

        //private static TileEffect PendingTileEffect;

        private CardDrawer _cardDrawer;
        private Random _playerRandomization;
        private ActionPlanner _actionPlanner;

        private Board Board { get; set; }
        public Guid Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public Dictionary<Guid, Player> Players { get; set; }

        internal async Task Initialize()
        {
            await Logger.Log("na", "Initializing...");
            _actionPlanner = new ActionPlanner();
            _actionPlanner.ActionFinished += _actionPlanner_ActionFinished;
            _cardDrawer = new CardDrawer();
            await _cardDrawer.LoadResources();

            _playerRandomization = new Random((int)(DateTime.Now.Millisecond / 3.5));

            Board = BoardGenerator.Instance.Original();
            PlayerTack = new PlayersTracking();
        }

        private async void _actionPlanner_ActionFinished(Player player)
        {
            await Logger.Log(player.Name, $"All actions choices done. Sending game update to all players");
            await UpdateGame(Hubconcentrator.Hub);
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
            if (Players.TryGetValue(playerId, out var player))
            {
                await Logger.Log(player.Name, $"Placing tile '{BoardTilesHandler.PendingTileEffect.Type}' on place '{place.Index}'");
                await BoardTilesHandler.PlaceTileOnBoard(place, player, BoardTilesHandler.PendingTileEffect, Board, _cardDrawer);
                BoardTilesHandler.PendingTileEffect = null;
                await _actionPlanner.Continue(player, Board);
            }
        }

        public async Task SelectActionTarget(ResourceEffectPlayerChooser place, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {

                if (place.TargetPlayerId != Guid.Empty && place.TargetPlayerId != _neutralPlayerId)
                {
                    if (Players.TryGetValue(playerId, out var targetPlayer))
                    {
                        await Logger.Log(player.Name, $"Selected action target : {targetPlayer.Name}");

                        await EffectHandler.HandleResourceEffect(targetPlayer, place.ResourceHandler,Players.Select(p=>p.Value).ToList(), Board);
                    }
                }

                await _actionPlanner.Continue(player, Board);
            }
        }
        public async Task BoardAction(BoardAction boardAction, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var choices = await ActionPlayHandler.ExecuteBoardAction(boardAction, Board, player);
                foreach (var choice in choices)
                {
                    _actionPlanner.Plan(choice);
                }
                await PlanCommon(hubContext, player);
            }
        }
        public async Task CardAction(Action action, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var choices = await ActionPlayHandler.ExecuteAction(action, Board, player, _cardDrawer);
                foreach (var choice in choices)
                {
                    _actionPlanner.Plan(choice);
                }
                await PlanCommon(hubContext, player);
            }
        }
        public async Task ConvertResources(ResourceHandler resources, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var playerObj))
            {
                var choices = await ActionPlayHandler.Convert(playerObj, resources, Board);
                foreach (var choice in choices)
                {
                    _actionPlanner.Plan(choice);
                }
                await PlanCommon(hubContext, playerObj);
            }
        }
        public async Task PlayCard(CardActionPlay card, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var choices = await ActionPlayHandler.Play(card, player, Board, Players.Select(r => r.Value).ToList());

                foreach (var choice in choices)
                {
                    _actionPlanner.Plan(choice);
                }
                await PlanCommon(hubContext, player);
            }
        }
        public async Task VerifyCard(Patent card, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var handCard = player.HandCards.FirstOrDefault(c => c.Guid == card.Guid);
                if (handCard != null)
                {
                    handCard.CanBePlayed = PrerequisiteHandler.CanPlayCard(card, Board, player);
                    await UpdateGame(hubContext);
                }
            }
        }
        public async Task VerifyCardWithResources(PlayCardWithResources modifiers, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(playerId, out var player))
            {
                var handCard = player.HandCards.FirstOrDefault(c => c.Guid == modifiers.Patent.Guid);
                if (handCard != null)
                {
                    await EffectHandler.CheckCardsReductions(player);
                    PrerequisiteHandler.VerifyResourcesUsage(handCard, modifiers.CardMineralModifiers, player);
                    handCard.CanBePlayed = PrerequisiteHandler.CanPlayCard(handCard, Board, player);
                    await UpdateGame(hubContext);
                }
            }
        }
        private async Task PlanCommon(IHubContext<ClientNotificationHub> hubContext, Player player)
        {
            _actionPlanner.Plan(async (p, b) =>
            {
                await EffectHandler.CheckCardsReductions(p);
                await _actionPlanner.Continue(p, b);
            });
            _actionPlanner.Plan(async (p, b) =>
            {
                await PrerequisiteHandler.CanPlayBoardAction(b, p);
                await _actionPlanner.Continue(p, b);
            });
            _actionPlanner.Plan(async (p, b) =>
            {
                await PrerequisiteHandler.CanPlayCardAction(b, p);
                await _actionPlanner.Continue(p, b);
            });
            _actionPlanner.Plan(async (p, b) =>
            {
                await PrerequisiteHandler.CanPlayCards(b, p);
                await _actionPlanner.Continue(p, b);
            });
            _actionPlanner.Plan(async (p, b) =>
            {
                await PrerequisiteHandler.CanConvertResources(b, p);
                await _actionPlanner.Continue(p, b);
            });
            _actionPlanner.Plan(async (p, b) =>
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
                    await _actionPlanner.Continue(p, b);
                }
            });
            //}

            await _actionPlanner.FollowPlan(player, Board);
        }

        public async Task<Player> SetupPlayer(GameSetupSelection selection, IHubContext<ClientNotificationHub> hubContext)
        {
            if (Players.TryGetValue(selection.PlayerId, out var player))
            {
                Corporation selectedCorpo = null;
                if (selection.Corporation != null && selection.Corporation.Any())
                {
                    var selected = selection.Corporation.FirstOrDefault(t => t.Value);
                    selectedCorpo = _cardDrawer.PickCorporation(Guid.Parse(selected.Key));
                    await Logger.Log(player.Name, $"Receiving player setup. chosen corporation = '{selectedCorpo.Name}'");
                    player.Corporation = selectedCorpo;
                    //foreach (var corporationEffect in selectedCorpo.ResourcesEffects)
                    //{
                    //    await EffectHandler.HandleResourceEffect(player, corporationEffect);
                    //}
                }

                await Logger.Log(player.Name, $"Patent bought : {selection.BoughtCards.Count}");

                await EffectHandler.HandleInitialPatentBuy(player, _cardDrawer.DispatchPatents(selection.BoughtCards), selectedCorpo,Board, Players.Select(p=>p.Value).ToList());

                await EffectHandler.CheckCardsReductions(player);
                await PrerequisiteHandler.CanPlayCards(Board, player);
                await PrerequisiteHandler.CanConvertResources(Board, player);
                await PrerequisiteHandler.CanPlayBoardAction(Board, player);

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
                    await SendPlayersPlaying(hubContext, playing);
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
                //energy to heat conversion
                player.Value[ResourceType.Heat].UnitCount += player.Value[ResourceType.Energy].UnitCount;
                player.Value[ResourceType.Energy].UnitCount = 0;

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

                foreach (var valueHandCard in player.Value.HandCards)
                {
                    valueHandCard.ActionPlayed = false;
                }
            }

            PlayerTack.NewGeneration();
            await SendSetup(hubContext, false);
            await UpdateGame(hubContext);
        }

        private async Task SendPlayersPlaying(IHubContext<ClientNotificationHub> hubContext, Guid playing)
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

                await SendPlayersPlaying(hubContext, playing);
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

                for (int i = 0; i < (isInitialSetup ? 10 : 4); i++)
                {
                    gs.Patents.Add(_cardDrawer.DrawPatent());
                }

                return gs;
            }
            return null;
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