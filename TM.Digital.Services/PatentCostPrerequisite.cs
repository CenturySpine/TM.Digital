using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    public class PatentCostPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            var pleyrsMoney = patentOwner.Resources.First(r => r.ResourceType == ResourceType.Money).UnitCount;
            bool cashCostRequired = pleyrsMoney >= inputPatent.ModifiedCost;

            var allCards = patentOwner.PlayedCards.Concat(new List<Card> { patentOwner.Corporation }).ToList();

            var titaniumModifier = 3 + allCards.Where(r => r.MineralModifiers?.TitaniumModifier?.Value > 0).Sum(t => t.MineralModifiers?.TitaniumModifier?.Value);
            var steelModifier = 2 + allCards.Where(r => r.MineralModifiers?.SteelModifier?.Value > 0).Sum(t => t.MineralModifiers?.SteelModifier?.Value);

            var titaniumModifiedMoney = patentOwner.Resources.First(r => r.ResourceType == ResourceType.Titanium).UnitCount * titaniumModifier;
            var steelModifiedMoney = patentOwner.Resources.First(r => r.ResourceType == ResourceType.Steel).UnitCount * steelModifier;

            var resourceCostRequired = pleyrsMoney
                                       + ((inputPatent.Tags.Any(t => t == Tags.Space)) ? titaniumModifiedMoney : 0)
                                       + ((inputPatent.Tags.Any(t => t == Tags.Building)) ? steelModifiedMoney : 0)
                                       >= inputPatent.ModifiedCost;

            return cashCostRequired || resourceCostRequired;
        }
    }
}