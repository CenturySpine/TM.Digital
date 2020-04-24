using System.Collections.Generic;

namespace TM.Digital.Model.Cards
{
    public class Prerequisites
    {
        public List<TagsPrerequisite> TagsPrerequisites { get; set; }
        public List<GlobalPrerequisite> GlobalPrerequisites { get; set; }
    }
}