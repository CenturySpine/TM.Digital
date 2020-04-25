using System;

namespace TM.Digital.Model.Tile
{
    public class Tile
    {
        public TileType Type { get; set; }
        public Guid? Owner { get; set; }
        public int BoardIndex { get; set; }

        public Tile Clone()
        {
            return new Tile
            {
                Owner = Owner,
                BoardIndex = BoardIndex,
                Type = Type
            };
        }
    }
}