using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class ConversionRate : Effect
    {
        public ResourceType ResourceType { get; set; }
        public int Rate { get; set; }
    }
}