using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Board
{
    public class BoardPlace
    {
        public BoardPlace()
        {
            Reserved = new BoardPlaceReservedSpace { IsExclusive = false, ReservedFor = ReservedFor.None };
            PlacementBonus = new List<ResourceEffect>();
        }

        public string Name { get; set; }

        public PlaceCoordinates Index { get; set; }
        public BoardPlaceReservedSpace Reserved { get; set; }

        public List<ResourceEffect> PlacementBonus { get; set; }

        public Tile.Tile PlayedTile { get; set; }

        public bool CanBeChoosed { get; set; }

        public BoardPlace Clone()
        {
            return new BoardPlace
            {
                Name = Name,
                CanBeChoosed = CanBeChoosed,
                Index = new PlaceCoordinates { X = Index.X, Y = Index.Y },
                Reserved = Reserved.Clone(),
                PlacementBonus = new List<ResourceEffect>(PlacementBonus.Select(b => b.Clone())),
                PlayedTile = PlayedTile?.Clone()

            };
        }

        public void CopyFrom(BoardPlace copiedTile)
        {
            Reserved = copiedTile.Reserved.Clone();
            PlacementBonus = copiedTile.PlacementBonus != null ? new List<ResourceEffect>(copiedTile.PlacementBonus.Select(g => g.Clone())) : null;
        }
    }
}