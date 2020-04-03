using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public class ResourceUnitEffect : IEffect
    {
        public EffectType Type { get; set; } = EffectType.ResourceUnit;
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var resource = player.Resources.FirstOrDefault(r => r.ResourceType == ResourceType);

            if (resource != null)
            {
                resource.UnitCount += Amount;

                //never go below zero for resources units
                if (resource.UnitCount < 0)
                {
                    resource.UnitCount = 0;
                }
            }
        }
    }
}