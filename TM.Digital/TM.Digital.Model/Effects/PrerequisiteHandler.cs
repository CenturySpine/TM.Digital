using System;
using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Effects
{
    public static class PrerequisiteHandler
    {


        public static bool CanPlayCard(Patent patent, Board.Board board, Player.Player player)
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
            return player.Resources.First(r => r.ResourceType == ResourceType.Money).UnitCount >= patent.BaseCost;
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