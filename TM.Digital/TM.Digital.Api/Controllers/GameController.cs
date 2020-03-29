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
        [Route("start")]
        public async Task Start()
        {
            await Task.CompletedTask;
            GameService.Instance.StartGame();
        }

        [Route("addplayer")]
        public async Task<GameSetup> AddPlayer()
        {
            await Task.CompletedTask;
           return GameService.Instance.AddPlayer();
        }

        [Route("addplayer/setup")]
        public async Task<Player> Setup(GameSetupSelection selection)
        {
            await Task.CompletedTask;
            return GameService.Instance.AddPlayer(selection);
        }
    }
}