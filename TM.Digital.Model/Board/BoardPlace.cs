using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public PlaceCoordinates Index { get; set; }
        public BoardPlaceReservedSpace Reserved { get; set; }

        public List<BoardPlaceBonus> PlacementBonus { get; set; }

        public Tile.Tile PlayedTile { get; set; }

        public bool CanBeChosed { get; set; }

        public BoardPlace Clone()
        {
            return new BoardPlace()
            {
                Name = Name,
                CanBeChosed = CanBeChosed,
                Index = new PlaceCoordinates() { X = Index.X, Y = Index.Y},
                Reserved =  Reserved.Clone(),
                PlacementBonus = new List<BoardPlaceBonus>(PlacementBonus.Select(b=>b.Clone())),
                PlayedTile = PlayedTile?.Clone()

            };
        }
    }
}