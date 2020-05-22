using System;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class ResourcesVictoryPoints : ICardVictoryPoints
    {
        public ResourceType ResourceType { get; set; }
        public int VictoryPointRatio { get; set; }

        public int Points { get; set; }

        public EffectModifierLocationConstraint Constraint { get; set; }

        public int VictoryPoint(Card card)
        {
            return Points * (int)Math.Floor((double)card.ResourcesCount / VictoryPointRatio);
        }

        public Tags Tag { get; set; }
    }


}