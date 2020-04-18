using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services.Common;

namespace TM.Digital.Services
{

    public static class EffectHandler
    {
        public static async Task HandleResourceEffect(Model.Player.Player player, ResourceEffect effect)
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
                    await Logger.Log(player.Name, $"Resource {resource.ResourceType} Production modified for {effect.Amount}, new value {resource.Production}");
                }
                else
                {
                    resource.UnitCount += effect.Amount;

                    if (resource.UnitCount < 0) resource.UnitCount = 0;
                    await Logger.Log(player.Name, $"Resource {resource.ResourceType} Unit modified for {effect.Amount}, new value {resource.UnitCount}");
                }
            }

        }

        public static async Task HandleInitialPatentBuy(Model.Player.Player player, List<Patent> selectionBoughtCards,
            Corporation selectionCorporation)
        {
            var playersMoney = player.Resources.First(r => r.ResourceType == ResourceType.Money);
            //if (selectionCorporation != null)
            //{
            //    playersMoney.UnitCount = selectionCorporation.StartingMoney;
            //    await Logger.Log(player.Name, $"Initial money count {selectionCorporation.StartingMoney}");
            //}

            foreach (var selectionBoughtCard in selectionBoughtCards)
            {
                playersMoney.UnitCount -= 3;
                player.HandCards.Add(selectionBoughtCard);
                await Logger.Log(player.Name, $"Buying patent '{selectionBoughtCard.Name}', remaining money {playersMoney.UnitCount}");
            }
        }

        public static async Task EvaluateResourceModifiers(Player player)
        {
            await Task.CompletedTask;
            var allCards = player.PlayedCards.Concat(new List<Card> { player.Corporation }).ToList();

            var titaniumModifier = 3 + allCards.Where(r => r.MineralModifiers?.TitaniumModifier?.Value > 0).Sum(t => t.MineralModifiers?.TitaniumModifier?.Value);
            var steelModifier = 2 + allCards.Where(r => r.MineralModifiers?.SteelModifier?.Value > 0).Sum(t => t.MineralModifiers?.SteelModifier?.Value);

            if (steelModifier != null)
                player.Resources.First(r => r.ResourceType == ResourceType.Steel).MoneyValueModifier =
                    steelModifier.Value;
            if (titaniumModifier != null)
                player.Resources.First(r => r.ResourceType == ResourceType.Titanium).MoneyValueModifier =
                    titaniumModifier.Value;
        }
        public static async Task CheckCardsReductions(Model.Player.Player player)
        {
            await EvaluateResourceModifiers(player);

            var reductions = player.PlayedCards.Concat(new List<Card> { player.Corporation })
                .SelectMany(c => c.TagEffects).Where(te => te.TagEffectType == TagEffectType.CostAlteration)
                .ToList();

            foreach (var playerHandCard in player.HandCards)
            {
                var reductionForCard = reductions.Where(r => playerHandCard.Tags.Any(t => r.AffectedTags.Contains(t)))
                    .ToList();
                if (reductionForCard.Any())
                {
                    await Logger.Log(player.Name, $"Updating players '{player.Name}' cards costs...");
                    foreach (var tagEffect in reductionForCard)
                    {
                        playerHandCard.ModifiedCost = playerHandCard.BaseCost + tagEffect.ResourceEffects.Sum(r => r.Amount);
                        if (playerHandCard.ModifiedCost < 0) playerHandCard.ModifiedCost = 0;
                        await Logger.Log(player.Name, $"New card '{playerHandCard.Name}' cost => {playerHandCard.ModifiedCost}");
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