using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Prerequisite
{
    public class Prerequisite
    {
        public int Value { get; set; }

        public PrerequisiteKind PrerequisiteKind { get; set; }
        public bool IsMax { get; set; }
    }

    public class ResourcesPrerequisite : Prerequisite
    {
        public ResourceType ResourceType { get; set; }
        public ResourceKind ResourceKind { get; set; }

    }

    public class TilePrerequisite : Prerequisite
    {
        public TileType TileType { get; set; }

        public EffectModifierLocationConstraint LocationConstraint { get; set; }
    }
}
