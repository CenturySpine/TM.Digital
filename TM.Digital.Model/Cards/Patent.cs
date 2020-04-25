using System.Collections.Generic;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class Patent : Card
    {
        public int BaseCost { get; set; }
        public int ModifiedCost { get; set; }

        public Prerequisites Prerequisites { get; set; }
        public bool CanBePlayed { get; set; }

        public int TitaniumUnitUsed { get; set; }
        public int SteelUnitUsed { get; set; }

        public bool IsSameCost
        {
            get { return BaseCost == ModifiedCost; }
        }
    }

    public class PlayCardWithResources
    {
        public Patent Patent { get; set; }
        public List<ActionPlayResourcesUsage> CardMineralModifiers { get; set; }
    }


}