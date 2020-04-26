using System;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    /// <summary>
    /// Check that if the card requires to reduce player's own resource production, the player has enough production in this resource
    /// </summary>
    public class ResourcesProductionReductionPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            var resourcesUnitCost = inputPatent.ResourcesEffects
                .Where(r => r.ResourceKind == ResourceKind.Production
                            && r.EffectDestination == ActionTarget.Self && r.Amount < 0)
                .ToList();

            foreach (var resourceCost in resourcesUnitCost)
            {
                var playerResource = patentOwner.Resources.First((r => r.ResourceType == resourceCost.ResourceType));
                var differential = playerResource.Production - Math.Abs(resourceCost.Amount);
                if (playerResource.ResourceType != ResourceType.Money)
                {
                    if (differential < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (differential < -5)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}