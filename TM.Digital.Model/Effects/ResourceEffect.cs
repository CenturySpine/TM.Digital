using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Effects
{
    public class ResourceEffect : Effect
    {
        public ResourceKind ResourceKind { get; set; }
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public ActionTarget EffectDestination { get; set; }

        public EffectModifier EffectModifier { get; set; }
    }

    public class EffectModifier
    {
        public Tags TagsModifier { get; set; }

        public TileType TileModifier { get; set; }

        public ActionTarget ModifierFrom { get; set; }

        public int ModifierRatio { get; set; }

        public EffectModifierLocationConstraint EffectModifierLocationConstraint { get; set; }
    }

    public enum EffectModifierLocationConstraint
    {
        None,
        Anywhere, 
        OnMars
    }

    public class SpecialAction
    {
        private int CostModifier { get; set; }

        public int CardDrawNumber { get; set; }

        public int CardKeepNumber { get; set; }
    }
    public enum SpecialActionType
    {
        None,

    }
}