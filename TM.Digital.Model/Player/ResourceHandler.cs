using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Player
{
    public class ResourceHandler
    
    {
        public ResourceType ResourceType { get; set; }
        public int UnitCount { get; set; }
        public int Production { get; set; }

        public int MoneyValueModifier { get; set; }

    }
}