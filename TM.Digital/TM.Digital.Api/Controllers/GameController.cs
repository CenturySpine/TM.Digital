using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Api.Hubs;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Services;

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
        [Route("start/{numberofplayer:int}")]
        public async Task<Guid> Start(int numberofplayer)
        {
            await Task.CompletedTask;
            return GamesService.Instance.StartGame(numberofplayer);
        }

        [Route("addplayer/{gameId}/{playername}")]
        public async Task<GameSetup> AddPlayer(Guid gameId, string playername)
        {
            await Task.CompletedTask;
            return GamesService.Instance.AddPlayer(gameId, playername);
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
            await Task.CompletedTask;
            var playResult = GamesService.Instance.Play(card, gameId, playerId);
            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", "PlayResult", JsonSerializer.Serialize(playResult));
            return Ok();
        }
    }


}