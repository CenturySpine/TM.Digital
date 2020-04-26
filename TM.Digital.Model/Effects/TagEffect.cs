using System.Collections.Generic;

namespace TM.Digital.Model.Effects
{
    public class TagEffect : Effect
    {
        public TagEffectType TagEffectType { get; set; }
        
        public ActionOrigin ActionOrigin { get; set; }

        public TagsList AffectedTags { get; set; }
        public MultipleEffectCombinationType CombinationType { get; set; }

        public List<ResourceEffect> ResourceEffects { get; set; }
    }
}