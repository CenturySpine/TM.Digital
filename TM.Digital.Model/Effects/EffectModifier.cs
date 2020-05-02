using TM.Digital.Model.Cards;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Effects
{
    public class EffectModifier
    {
        public Tags TagsModifier { get; set; }

        public TileType TileModifier { get; set; }

        public ActionTarget ModifierFrom { get; set; }

        public int ModifierRatio { get; set; }

        public EffectModifierLocationConstraint EffectModifierLocationConstraint { get; set; }

        public EffectModifier Clone()
        {
            return new EffectModifier()
                {
                    TileModifier = TileModifier,
                    ModifierFrom = ModifierFrom,
                    ModifierRatio =  ModifierRatio,
                    EffectModifierLocationConstraint = EffectModifierLocationConstraint,
                    TagsModifier = TagsModifier
            };
        }
    }
}