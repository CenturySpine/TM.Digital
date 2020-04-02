using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public class ResourceEffect : IEffect
    {
        public ResourceKind ResourceKind { get; set; }
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            var resource = player.PlayerBoard.Resources.FirstOrDefault(r => r.ResourceType == ResourceType);

            if (resource != null)
            {
                resource.Production += Amount;

                //never go below -5 for money production
                if (resource.ResourceType == ResourceType.Money && resource.Production < -5)
                {
                    resource.Production = -5;
                }
                //never go below zero for other resources production
                else if (resource.Production < 0)
                {
                    resource.Production = 0;
                }
            }
        }
    }
}