using TM.Digital.Model.Effects;

namespace TM.Digital.Model.Tile
{
    public class TileEffect: Effect
    {
        public TileType Type { get; set; }
        public int Number { get; set; }
        public TilePlacementCosntrains Constrains { get; set; }
    }
}