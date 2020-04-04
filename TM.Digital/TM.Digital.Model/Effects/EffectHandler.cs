using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public static class EffectHandler
    {
        public static void HandleResourceEffect(Player.Player player, ResourceEffect effect)
        {
            if (effect.EffectDestination == EffectDestination.Self)
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
                        else if (resource.ResourceType != ResourceType.Money && resource.Production < 0)
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
            else
            {
                //TODO
            }
        }

        public static void HandleInitialPatentBuy(Player.Player player, List<Patent> selectionBoughtCards,
            Corporation selectionCorporation)
        {
            var playersMoney = player.Resources.First(r => r.ResourceType == ResourceType.Money);
            playersMoney.UnitCount = selectionCorporation.StartingMoney;
            foreach (var selectionBoughtCard in selectionBoughtCards)
            {
                playersMoney.UnitCount -= 3;
                player.HandCards.Add(selectionBoughtCard);
            }
        }

        public static void CheckCardsReductions(Player.Player player)
        {
            var reductions = player.PlayedCards.Concat(new List<Card> { player.Corporation })
                .SelectMany(c => c.TagEffects).Where(te => te.TagEffectType == TagEffectType.CostAlteration)
                .ToList();

            foreach (var playerHandCard in player.HandCards)
            {
                var reductionForCard = reductions.Where(r => playerHandCard.Tags.Contains(r.AffectedTag))
                    .ToList();
                if (reductionForCard.Any())
                {
                    foreach (var tagEffect in reductionForCard)
                    {
                        playerHandCard.ModifiedCost = playerHandCard.BaseCost += tagEffect.EffectValue;
                    }

                }
                else
                {
                    playerHandCard.ModifiedCost = playerHandCard.BaseCost;
                }
            }
        }
    }
}