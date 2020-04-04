using System;
using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public static class PrerequisiteHandler
    {

        public static void CanPlayCards(Board.Board board, Player.Player player)
        {
            foreach (var playerHandCard in player.HandCards)
            {
                playerHandCard.CanBePlayed = CanPlayCard(playerHandCard, board, player);
            }
        }

        private static bool CanPlayCard(Patent patent, Board.Board board, Player.Player player)
        {
            return CheckGlobalPrerequisite(patent, board)
                   && CheckPatentCost(player, patent)
                   && CheckResourcesProductionReduction(patent, player)
                   && CheckResourcesUnitReduction(patent, player)
                   && CheckTags(player, patent);
        }

        private static bool CheckTags(Player.Player player, Patent patent)
        {
            var playerTagsCount = player.PlayedCards
                .Where(c => c.CardType != CardType.Red)
                .SelectMany(c => c.Tags)
                .GroupBy(r => r)
                .Select(grp => new { Tag = grp.Key, Count = grp.Count() })
                .ToList();

            foreach (var patentTagsPrerequisite in patent.TagsPrerequisites)
            {
                var playerTagsMatch = playerTagsCount.FirstOrDefault(t => t.Tag == patentTagsPrerequisite.Tag);
                if (playerTagsMatch == null || playerTagsMatch.Count < patentTagsPrerequisite.Value)
                    return false;
            }

            return true;
        }

        private static bool CheckResourcesProductionReduction(Patent patent, Player.Player player)
        {
            var resourcesProductCost = patent.ResourcesEffects.Where(r => r.ResourceKind == ResourceKind.Production && r.EffectDestination == EffectDestination.Self && r.Amount < 0).ToList();
            foreach (var resourceCost in resourcesProductCost)
            {
                var playerResource = player.Resources.First((r => r.ResourceType == resourceCost.ResourceType));
                if (playerResource.Production < Math.Abs(resourceCost.Amount))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckResourcesUnitReduction(Patent patent, Player.Player player)
        {
            var resourcesUnitCost = patent.ResourcesEffects.Where(r => r.ResourceKind == ResourceKind.Unit && r.EffectDestination == EffectDestination.Self && r.Amount < 0).ToList();
            foreach (var resourceCost in resourcesUnitCost)
            {
                var playerResource = player.Resources.First((r => r.ResourceType == resourceCost.ResourceType));
                if (playerResource.UnitCount < Math.Abs(resourceCost.Amount))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckPatentCost(Player.Player player, Patent patent)
        {
            var pleyrsMoney = player.Resources.First(r => r.ResourceType == ResourceType.Money).UnitCount;
            bool cashCostRequired = pleyrsMoney >= patent.ModifiedCost;

            var allCards = player.PlayedCards.Concat(new List<Card>() {player.Corporation}).ToList();

            var titaniumModifier = 3 + allCards.Where(r => r.TitaniumValueModifier > 0).Sum(t => t.TitaniumValueModifier);
            var steelModifier = 2 + allCards.Where(r => r.SteelValueModifier > 0).Sum(t => t.SteelValueModifier);

            var titaniumModifiedMoney = player.Resources.First(r => r.ResourceType == ResourceType.Titanium).UnitCount * titaniumModifier;
            var steelModifiedMoney = player.Resources.First(r => r.ResourceType == ResourceType.Steel).UnitCount * steelModifier;

            var resourceCostRequired = pleyrsMoney + titaniumModifiedMoney + steelModifiedMoney >= patent.ModifiedCost;

            return cashCostRequired || resourceCostRequired;
        }

        private static bool CheckGlobalPrerequisite(Patent patent, Board.Board board)
        {
            foreach (var patentPrerequisite in patent.GlobalPrerequisites)
            {
                var boardParam = board.Parameters.First(p => p.Type == patentPrerequisite.Parameter);

                if (patentPrerequisite.IsMax && boardParam.GlobalParameterLevel.Level > patentPrerequisite.Value)
                    return false;
                if (patentPrerequisite.Value < boardParam.GlobalParameterLevel.Level)
                    return false;
            }

            return true;
        }


    }
}