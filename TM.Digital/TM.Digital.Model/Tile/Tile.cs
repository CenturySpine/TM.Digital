using System;

namespace TM.Digital.Model.Tile
{
    public class Tile
    {
        public TileType Type { get; set; }
        public Guid? Owner { get; set; }
        public int BoardIndex { get; set; }
    }
}