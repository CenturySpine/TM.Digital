using System;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    /// <summary>
    /// Check that if the card requires to reduce player's own resource units, the player has enough units in this resource
    /// </summary>
    public class ResourcesUnitReductionPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            var resourcesUnitCost = inputPatent.ResourcesEffects
                .Where(r => r.ResourceKind == ResourceKind.Unit
                            && r.EffectDestination == ActionTarget.Self && r.Amount < 0)
                .ToList();

            foreach (var resourceCost in resourcesUnitCost)
            {
                var playerResource = patentOwner.Resources.First((r => r.ResourceType == resourceCost.ResourceType));
                if (playerResource.UnitCount < Math.Abs(resourceCost.Amount))
                {
                    return false;
                }
            }

            return true;
        }
    }
}