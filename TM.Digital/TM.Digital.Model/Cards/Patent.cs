using System.Collections.Generic;

namespace TM.Digital.Model.Cards
{
    public class Patent : Card
    {
        public int BaseCost { get; set; }
        public int ModifiedCost { get; set; }

        public List<TagsPrerequisite> TagsPrerequisites { get; set; } = new List<TagsPrerequisite>();
        public List<GlobalPrerequisite> GlobalPrerequisites { get; set; } = new List<GlobalPrerequisite>();
        public bool CanBePlayed { get; set; }
    }
}