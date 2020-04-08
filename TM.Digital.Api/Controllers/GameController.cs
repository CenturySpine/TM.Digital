﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Services;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Api.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<ClientNotificationHub> _hubContext;

        public GameController(IHubContext<ClientNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Route("create/{playerName}/{numberofplayer:int}")]
        public async Task<GameSessionInformation> Start(string playerName, int numberofplayer)
        {
            await Task.CompletedTask;
            return GamesService.Instance.CreateGame(playerName, numberofplayer);
        }

        [Route("sessions")]
        public async Task<GameSessions> GetExistingSessions()
        {
            await Task.CompletedTask;
            return GamesService.Instance.GetSessions();
        }

        [Route("join/{gameId}/{playername}")]
        public async Task<Player> JoinGame(Guid gameId, string playerName)
        {
            
            return await GamesService.Instance.JoinSession(gameId, playerName, _hubContext);
        }



        [Route("addplayer/{gameId}/{playername}/{test}")]
        public async Task<GameSetup> AddPlayer(Guid gameId, string playername, bool test)
        {
            await Task.CompletedTask;
            return GamesService.Instance.AddPlayer(gameId, playername, test);
        }

        [Route("addplayer/setupplayer")]
        public async Task<Player> SetupPlayer(GameSetupSelection selection)
        {
            await Task.CompletedTask;
            return GamesService.Instance.SetupPlayer(selection);
        }

        [Route("{gameId}/play/{playerId}")]
        public async Task<ActionResult> PlayCard(Patent card, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.Play(card, gameId, playerId, _hubContext);

            return Ok();
        }

        [Route("{gameId}/placetile/{playerId}")]
        public async Task<ActionResult> PlaceTile(BoardPlace place, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.PlaceTile(place, gameId, playerId, _hubContext);

            return Ok();
        }
    }
}