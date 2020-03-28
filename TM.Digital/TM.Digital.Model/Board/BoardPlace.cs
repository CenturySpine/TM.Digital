using System.Collections.Generic;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Board
{
    public class BoardPlace
    {
        public BoardPlace()
        {
            Reserved = new BoardPlaceReservedSpace { IsExclusive = false, ReservedFor = ReservedFor.None };
            PlacementBonus = new List<BoardPlaceBonus>();
        }

        public string Name { get; set; }

        public int Index { get; set; }
        public BoardPlaceReservedSpace Reserved { get; set; }

        public List<BoardPlaceBonus> PlacementBonus { get; set; }
    }

    public class BoardLine 
    {
        public int Index { get; set; }
        public List<BoardPlace> BoardPlaces { get; set; }
    }
}