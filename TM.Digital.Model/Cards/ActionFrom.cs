using System.Collections.Generic;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class ActionFrom
    {
        public ActionTarget ActionTarget { get; set; }

        public ResourceEffect ResourceEffect { get; set; }
        public ResourceEffectAlternatives ResourceEffectsAlternatives { get; set; }

        public ResourceType ResourceTypeAlternativePayment { get; set; }
        //public int Amount { get; set; }
        //public ResourceKind ResourceKind { get; set; }
    }
}