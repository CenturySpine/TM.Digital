using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Services;
using TM.Digital.Services.Common;
using TM.Digital.Transport.Hubs.Hubs;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Api.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<ClientNotificationHub> _hubContext;

        public GameController(IHubContext<ClientNotificationHub> hubContext)
        {
            Logger.HubContext = Hubconcentrator.Hub = hubContext;
            _hubContext = hubContext;
        }

        [Route("create/{playerName}/{numberofplayer:int}")]
        public async Task<GameSessionInformation> CreateGame(string playerName, int numberofplayer)
        {
            return await GamesService.Instance.CreateGame(playerName, numberofplayer);
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

        [Route("start/{gameId}")]
        public async Task<bool> Start(Guid gameId)
        {
            return await GamesService.Instance.StartGame(gameId, _hubContext);
        }

        [Route("addplayer/setupplayer")]
        public async Task<Player> SetupPlayer(GameSetupSelection selection)
        {
            return await GamesService.Instance.SetupPlayer(selection, _hubContext);
        }

        [Route("{gameId}/play/{playerId}")]
        public async Task<ActionResult> PlayCard(CardActionPlay card, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.Play(card, gameId, playerId, _hubContext);

            return Ok();
        }

        [Route("{gameId}/boardaction/{playerId}")]
        public async Task<ActionResult> BoardAction(BoardAction boardAction, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.BoardAction(boardAction, gameId, playerId, _hubContext);

            return Ok();
        }
        [Route("{gameId}/cardaction/{playerId}")]
        public async Task<ActionResult> CardAction(Action action, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.CardAction(action, gameId, playerId, _hubContext);

            return Ok();
        }

        [Route("{gameId}/verify/{playerId}")]
        public async Task<ActionResult> VerifyCard(Patent card, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.VerifyCard(card, gameId, playerId, _hubContext);

            return Ok();
        }
        [Route("{gameId}/verifywithresources/{playerId}")]
        public async Task<ActionResult> VerifyCardWithResources(PlayCardWithResources modifiers, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.VerifyCardWithResources(modifiers, gameId, playerId, _hubContext);

            return Ok();
        }

        [Route("{gameId}/pass/{playerId}")]
        public async Task<bool> Pass(Guid gameId, Guid playerId)
        {
            return await GamesService.Instance.Pass(gameId, playerId, _hubContext);
        }

        [Route("{gameId}/skip/{playerId}")]
        public async Task<bool> Skip(Guid gameId, Guid playerId)
        {
            return await GamesService.Instance.Skip(gameId, playerId, _hubContext);
        }

        [Route("{gameId}/placetile/{playerId}")]
        public async Task<ActionResult> PlaceTile(BoardPlace place, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.PlaceTile(place, gameId, playerId, _hubContext);

            return Ok();
        }

        [Route("{gameId}/convert/{playerId}")]
        public async Task<ActionResult> ConvertResources(ResourceHandler resources, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.ConvertResources(resources, gameId, playerId, _hubContext);

            return Ok();
        }

        [Route("{gameId}/selectactiontarget/{playerId}")]
        public async Task<ActionResult> SelectActionTarget(ResourceEffectPlayerChooser place, Guid gameId, Guid playerId)
        {
            await GamesService.Instance.SelectActionTarget(place, gameId, playerId, _hubContext);

            return Ok();
        }
    }
}