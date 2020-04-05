using System;
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

        public PlaceCoordinates Index { get; set; }
        public BoardPlaceReservedSpace Reserved { get; set; }

        public List<BoardPlaceBonus> PlacementBonus { get; set; }

        public Tile.Tile PlayedTile { get; set; }

        public bool CanBeChosed { get; set; }
    }

    public class PlaceCoordinates
    {
        protected bool Equals(PlaceCoordinates other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaceCoordinates) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}