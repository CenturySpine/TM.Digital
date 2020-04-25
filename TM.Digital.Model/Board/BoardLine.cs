using System.Collections.Generic;
using System.Linq;

namespace TM.Digital.Model.Board
{
    public class BoardLine
    {
        public int Index { get; set; }
        public List<BoardPlace> BoardPlaces { get; set; }

        public BoardLine Clone()
        {
            return new BoardLine()
            {
                Index = Index,
                BoardPlaces = new List<BoardPlace>(BoardPlaces.Select(p => p.Clone()))
            };
        }
    }
}