using System.Collections.Generic;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Cards
{
    public class ActionTo
    {
        //public ActionTarget ActionTarget { get; set; }



        public BoardLevelEffect BoardLevelEffect { get; set; }

        public List<ResourceEffect> ResourceEffectsAlternatives { get; set; }
        public ResourceEffect ResourceEffect { get; set; }

        public TileEffect TileEffect { get; set; }
    }
}