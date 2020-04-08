using Microsoft.AspNetCore.Mvc;
using TM.Digital.Model.Board;
using TM.Digital.Services;

namespace TM.Digital.Api.Controllers
{
    [Route("api/marsboard")]
    [ApiController]
    public class MarsBoardController : ControllerBase
    {
        [Route("original")]
        public Board Original()
        {
            var genBoard= BoardGenerator.Instance.Original();
            return genBoard;
        }
    }
}