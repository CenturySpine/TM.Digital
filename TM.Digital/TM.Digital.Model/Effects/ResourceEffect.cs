using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{

    public enum EffectDestination
    {
        Self,
        OtherPlayer
    }
    public class ResourceEffect
    {
        public ResourceKind ResourceKind { get; set; }
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }

        public EffectDestination EffectDestination { get; set; }


    }

    public static class EffectHandler
    {
        public static void HandleResourceEffect(Player.Player player, ResourceEffect effect)
        {
            var resource = player.Resources.FirstOrDefault(r => r.ResourceType == effect.ResourceType);

            if (resource != null)
            {
                if (effect.ResourceKind == ResourceKind.Production)
                {
                    resource.Production += effect.Amount;

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
                else
                {
                    resource.UnitCount += effect.Amount;
                    if (resource.UnitCount < 0) resource.UnitCount = 0;
                }
            }
        }
    }
}