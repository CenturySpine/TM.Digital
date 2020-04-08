using System.Collections.Generic;

namespace TM.Digital.Model.Board
{
    public class BoardLine 
    {
        public int Index { get; set; }
        public List<BoardPlace> BoardPlaces { get; set; }
    }
}