using System;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Cards
{
    public class ResourcesVictoryPoints : ICardVictoryPoints
    {
        public ResourceType ResourceType { get; set; }
        public int VictoryPointRatio { get; set; }

        public int VictoryPoint(Card card)
        {
            return (int)Math.Floor((double)card.ResourcesCount / VictoryPointRatio);
        }
    }
}