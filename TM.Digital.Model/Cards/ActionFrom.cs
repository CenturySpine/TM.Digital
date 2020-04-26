using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class ActionFrom
    {
        public ActionTarget ActionTarget { get; set; }

        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }
        public ResourceKind ResourceKind { get; set; }
    }
}