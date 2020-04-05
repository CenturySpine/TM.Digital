using System.Collections.Generic;
using System.Drawing;
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

        public Point Index { get; set; }
        public BoardPlaceReservedSpace Reserved { get; set; }

        public List<BoardPlaceBonus> PlacementBonus { get; set; }

        public Tile.Tile PlayedTile { get; set; }
    }
}