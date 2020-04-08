using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public class ResourceEffect
    {
        public ResourceKind ResourceKind { get; set; }
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public EffectDestination EffectDestination { get; set; }


    }
}