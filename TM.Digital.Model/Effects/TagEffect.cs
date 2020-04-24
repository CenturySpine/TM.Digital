using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Effects
{
    public enum MultipleEffectCombinationType
    {
        Or,
        And
    }

    public enum ActionOrigin
    {
        Self, 
        Any
    }
    public class TagEffect : Effect
    {
        public TagEffectType TagEffectType { get; set; }
        
        public ActionOrigin ActionOrigin { get; set; }

        public TagsList AffectedTags { get; set; }
        public MultipleEffectCombinationType CombinationType { get; set; }

        public List<ResourceEffect> ResourceEffects { get; set; }
    }

    public class TagsList : List<Tags>
    {
    }

    public class TilePassiveEffect : Effect
    {
        public TileType TileType { get; set; }

        public ActionOrigin ActionOrigin { get; set; }

        public List<ResourceEffect> ResourceEffects { get; set; }
    }

    public class TilePassiveEffects : List<TilePassiveEffect>
    {

    }
}