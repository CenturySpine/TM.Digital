using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public class ResourceEffect : Effect
    {
        public ResourceKind ResourceKind { get; set; }
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public ActionTarget EffectDestination { get; set; }

        public EffectModifier EffectModifier { get; set; }
    }
}