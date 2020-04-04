using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public class TagEffect : IEffect
    {
        public TagEffectType TagEffectType { get; set; }
        public Tags AffectedTag { get; set; }
        public int EffectValue { get; set; }
        public ResourceType ResourceType { get; set; }
        public ResourceKind ResourceKind { get; set; }

        

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var tagCount = card.Tags.Count(t => t == AffectedTag);
            var res = player.Resources.FirstOrDefault(r => r.ResourceType == ResourceType);
            if (ResourceKind == ResourceKind.Unit)
            {
                if (res != null) res.UnitCount += tagCount * EffectValue;
            }
            else
            {
                if (res != null) res.Production += tagCount * EffectValue;
            }
        }
    }
}