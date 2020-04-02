using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Services;

namespace TM.Digital.Api.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [Route("start/{numberofplayer:int}")]
        public async Task<bool> Start(int numberofplayer)
        {
            await Task.CompletedTask;
            return GameService.Instance.StartGame(numberofplayer);
        }

        [Route("addplayer/{playername}")]
        public async Task<GameSetup> AddPlayer(string playername)
        {
            await Task.CompletedTask;
           return GameService.Instance.AddPlayer(playername);
        }

        [Route("addplayer/setup")]
        public async Task<Player> Setup(GameSetupSelection selection)
        {
            await Task.CompletedTask;
            return GameService.Instance.AddPlayer(selection);
        }
    }
}