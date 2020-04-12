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
    }

    public class Prerequisites
    {
        public List<TagsPrerequisite> TagsPrerequisites { get; set; }
        public List<GlobalPrerequisite> GlobalPrerequisites { get; set; }
    }

    public class ActionPlay
    {
        public Patent Patent { get; set; }

        public List<ActionPlayResourcesUsage> ResourcesUsages { get; set; }
    }

    public class ActionPlayResourcesUsage
    {
        public ResourceType ResourceType { get; set; }

        public int UnitPlayed { get; set; }
    }
}