using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Services.Common;
using TM.Digital.Transport.Hubs;
using TM.Digital.Transport.Hubs.Hubs;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Services
{
    public class GamesService
    {
        public static GamesService Instance { get; } = new GamesService();

        private readonly Dictionary<Guid, GameSession> _currentSessions = new Dictionary<Guid, GameSession>();

        public async Task<GameSessionInformation> CreateGame(string playerName, int numberOfPlayer)
        {
            GameSession gs = new GameSession { Id = Guid.NewGuid(), OwnerName = playerName, Players = new Dictionary<Guid, Player>() };

            await gs.Initialize();

            gs.NumberOfPlayers = numberOfPlayer;

            var owner = gs.AddPlayer(playerName, false);

            gs.OwnerId = owner.PlayerId;
            _currentSessions.Add(gs.Id, gs);

            return
                new GameSessionInformation
                {
                    Owner = gs.OwnerName,
                    OwnerId = gs.OwnerId,
                    GameSessionId = gs.Id,
                    NumnerOfPlayers = gs.NumberOfPlayers
                };
        }

        public async Task<Player> SetupPlayer(GameSetupSelection selection, IHubContext<ClientNotificationHub> hubContext)
        {
            try
            {
                if (_currentSessions.TryGetValue(selection.GameId, out var session))
                {
                    return await session.SetupPlayer(selection, hubContext);
                }

                throw Errors.ErrorGameIdNotFound(selection.GameId);
            }
            catch (Exception e)
            {
                await Logger.Log("ERROR", e.ToString());
            }

            return null;
        }

        public async Task Play(ActionPlay card, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.PlayCard(card, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public async Task PlaceTile(BoardPlace place, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.PlaceTile(place, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public GameSessions GetSessions()
        {
            return new GameSessions
            {
                GameSessionsList = _currentSessions.Select(s => new GameSessionInformation
                {
                    Owner = s.Value.OwnerName,
                    OwnerId = s.Value.OwnerId,
                    GameSessionId = s.Key,
                    NumnerOfPlayers = s.Value.NumberOfPlayers
                })
                    .ToList()
            };
        }

        public async Task<Player> JoinSession(Guid gameId, string playerName, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                //game full
                if (session.Players.Count == session.NumberOfPlayers)
                    return null;

                var player = session.AddPlayer(playerName, false);
                if (player != null)
                {
                    await hubContext.Clients.All.SendAsync(ServerPushMethods.PlayerJoined, player.PlayerId, playerName);
                    return player;
                }
            }

            return null;
        }

        public async Task<bool> StartGame(Guid gameId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                return await session.Start(hubContext);
            }

            return false;
        }

        public async Task<bool> Skip(Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                return await session.Skip(playerId,hubContext);
            }

            return false;
        }

        public async Task<bool> Pass(Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                return await session.Pass(playerId, hubContext);
            }

            return false;
        }

        public async Task SelectActionTarget(
            ResourceEffectPlayerChooser place, 
            Guid gameId, 
            Guid playerId, 
            IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.SelectActionTarget(place, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public async Task ConvertResources(ResourceHandler resources, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.ConvertResources(resources, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);

        }

        public async Task VerifyCard(Patent card, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.VerifyCard(card, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public async Task VerifyCardWithResources(PlayCardWithResources modifiers, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.VerifyCardWithResources(modifiers, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);

        }

        public async Task BoardAction(BoardAction boardAction, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.BoardAction(boardAction, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }

        public async Task CardAction(Action action, Guid gameId, Guid playerId, IHubContext<ClientNotificationHub> hubContext)
        {
            if (_currentSessions.TryGetValue(gameId, out var session))
            {
                await session.CardAction(action, playerId, hubContext);
                return;
            }

            throw Errors.ErrorGameIdNotFound(gameId);
        }
    }
}