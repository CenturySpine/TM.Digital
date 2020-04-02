using System.Linq;
using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Effects
{
    public class TagCardCostEffect : IEffect
    {
        public EffectType Type { get; set; } = EffectType.TagCost;
        public Tags AffectedTag { get; set; }
        public int CostEffect { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var tagCount = card.Tags.Count(t => t == AffectedTag);
            //card.ModifiedCost = card.BaseCost += CostEffect * tagCount;
        }
    }
}