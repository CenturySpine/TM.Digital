using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TM.Digital.Model.Board;
using TM.Digital.Services;

namespace TM.Digital.Api.Controllers
{
    [Route("api/marsboard")]
    [ApiController]
    public class MarsBoardController : ControllerBase
    {
        [Route("{boardName}")]
        public Board Original(string boardName)
        {
            var genBoard= BoardGenerator.Instance.GetBoard(boardName);
            return genBoard;
        }
        [Route("boardslist")]
        public List<Board> ListBoards()
        {
            var genBoard = BoardGenerator.Instance.AllBoards.ToList();
            return genBoard;
        }
    }
}