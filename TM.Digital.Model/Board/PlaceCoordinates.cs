using System;

namespace TM.Digital.Model.Board
{
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

        public override string ToString()
        {
            return $"({nameof(X)}: {X}, {nameof(Y)}: {Y})";
        }
    }
}