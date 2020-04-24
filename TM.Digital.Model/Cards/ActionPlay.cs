using System.Collections.Generic;

namespace TM.Digital.Model.Cards
{
    public class ActionPlay
    {
        public Patent Patent { get; set; }

        public List<ActionPlayResourcesUsage> ResourcesUsages { get; set; }
    }
}