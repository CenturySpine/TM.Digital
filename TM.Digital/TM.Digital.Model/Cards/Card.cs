using System.Collections.Generic;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Prerequisite;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Cards
{
    public class Patent : Card
    {
        public int BaseCost { get; set; }
        public int ModifiedCost { get; set; }

        public List<TagsPrerequisite> TagsPrerequisites { get; set; } = new List<TagsPrerequisite>();
        public List<GlobalPrerequisite> GlobalPrerequisites { get; set; } = new List<GlobalPrerequisite>();
    }

    public class TagsPrerequisite : Prerequisite.Prerequisite
    {
        public Tags Tag { get; set; }

    }
    public class GlobalPrerequisite : Prerequisite.Prerequisite
    {
        public GlobalParameterType Parameter { get; set; }

    }
    public class Card
    {
        
        public string Name { get; set; }

        public List<Tags> Tags { get; set; }

        public CardType CardType { get; set; }

        public ResourceType ResourceType { get; set; }

        public int ResourcesCount { get; set; }



        public StandardVictoryPoint CardVictoryPoints { get; set; }
        public ResourcesVictoryPoints CardResourcesVictoryPoints { get; set; }

        public List<ResourceEffect> ResourcesEffects { get; set; } = new List<ResourceEffect>();
        public List<GlobalParameterLevelEffect> GlobalParameterEffects { get; set; } = new List<GlobalParameterLevelEffect>();

        public List<TagEffect> TagEffects { get; set; } = new List<TagEffect>();

        public List<TileEffect> TileEffects { get; set; } = new List<TileEffect>();

    }
}
