using System.Collections.Generic;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Effects
{
    public class TilePassiveEffect : Effect
    {
        public TileType TileType { get; set; }

        public ActionOrigin ActionOrigin { get; set; }

        public List<ResourceEffect> ResourceEffects { get; set; }
    }
}