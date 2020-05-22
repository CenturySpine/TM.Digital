using System.Collections.Generic;
using TM.Digital.Model.Prerequisite;

namespace TM.Digital.Model.Cards
{
    public class Prerequisites
    {
        public List<TagsPrerequisite> TagsPrerequisites { get; set; }
        public List<GlobalPrerequisite> GlobalPrerequisites { get; set; }
        public List<ResourcesPrerequisite> ResourcesPrerequisites { get; set; }

        public List<TilePrerequisite> TilePrerequisites { get; set; }
    }
}