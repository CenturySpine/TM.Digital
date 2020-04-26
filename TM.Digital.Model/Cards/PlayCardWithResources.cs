using System.Collections.Generic;

namespace TM.Digital.Model.Cards
{
    public class PlayCardWithResources
    {
        public Patent Patent { get; set; }
        public List<ActionPlayResourcesUsage> CardMineralModifiers { get; set; }
    }
}