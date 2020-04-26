using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services.Common;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Services
{
    public static class PrerequisiteHandler
    {
        public static async Task CanPlayCards(Model.Board.Board board, Model.Player.Player player)
        {
            await Logger.Log(player.Name, $"Updating players '{player.Name}' cards playability...");
            foreach (var playerHandCard in player.HandCards)
            {
                playerHandCard.CanBePlayed = CanPlayCard(playerHandCard, board, player);
            }
        }

        private static readonly List<IPrerequisiteStrategy> VerifyStrategies = new List<IPrerequisiteStrategy>
        {
            new GlobalCheckPrerequisite(),
            new PatentCostPrerequisite(),
            new ResourcesProductionReductionPrerequisite(),
            new ResourcesUnitReductionPrerequisite(),
            new TagsCountPrerequisite(),
            new TilePlacementRequirements()
        };

        internal static bool CanPlayCard(Patent patent, Model.Board.Board board, Model.Player.Player player)
        {
            return VerifyStrategies.All(t => t.CanPlayCard(patent, board, player));
        }

        public static async Task CanConvertResources(Board board, Player player)
        {
            await Task.CompletedTask;

            var plantConversion = GetPlantConversionRate(player);
            var heatConversion = GetHeatConversionRate(player);
            var plants = player[ResourceType.Plant];

            plants.CanConvert = plants.UnitCount >= plantConversion;


            var heat = player[ResourceType.Heat];

            heat.CanConvert = heat.UnitCount >= heatConversion;

        }

        public static int GetHeatConversionRate(Player player)
        {
            var cr = player.PlayedCards.Where(p => p.ConversionRates != null)
                .Select(pc => pc.ConversionRates).ToList();
            var heatConversion = cr.Any() ?
                cr.Where(p => p.Heat != null)
                    .Select(p => p.Heat)
                    .Min(t => t.Rate)
                : 0;
            if (heatConversion == 0) heatConversion = 8;
            return heatConversion;
        }

        public static int GetPlantConversionRate(Player player)
        {
            var cr = player.PlayedCards.Concat(new List<Card> { player.Corporation }).Where(p => p.ConversionRates != null)
                .Select(pc => pc.ConversionRates).ToList();

            var plantConversion = cr.Any() ?
                cr.Where(p => p.PlantConversion != null)
                    .Select(p => p.PlantConversion)
                    .Min(t => t.Rate)
                : 0;

            if (plantConversion == 0) plantConversion = 8;
            return plantConversion;
        }

        public static void VerifyResourcesUsage(Patent handCard, List<ActionPlayResourcesUsage> modifiersCardMineralModifiers, Player player)
        {
            var titaniumUsage =
                modifiersCardMineralModifiers.FirstOrDefault(t => t.ResourceType == ResourceType.Titanium);

            var steelUsage = modifiersCardMineralModifiers.FirstOrDefault(t => t.ResourceType == ResourceType.Steel);

            int titaniumUsageValue = 0;
            int steelUsageValue = 0;

            if (titaniumUsage != null)
            {
                var titaniumModofiers = player[ResourceType.Titanium].MoneyValueModifier;

                titaniumUsageValue = titaniumUsage.UnitPlayed * titaniumModofiers;
            }

            if (steelUsage != null)
            {
                var stellModifiers = player[ResourceType.Steel].MoneyValueModifier;

                steelUsageValue = steelUsage.UnitPlayed * stellModifiers;
            }

            handCard.SteelUnitUsed = steelUsage?.UnitPlayed ?? 0;
            handCard.TitaniumUnitUsed = titaniumUsage?.UnitPlayed ?? 0;
            handCard.ModifiedCost -= steelUsageValue + titaniumUsageValue;
            if (handCard.ModifiedCost < 0) handCard.ModifiedCost = 0;
        }


        public static async Task CanPlayBoardAction(Board board, Player player)
        {
            await Task.CompletedTask;
            foreach (var playerBoardAction in player.BoardActions)
            {
                playerBoardAction.Actions.CanExecute = CanPlayAction(player, playerBoardAction.Actions);
            }
        }

        public static bool CanPlayAction(Player player, Action playerBoardAction)
        {
            if (playerBoardAction.ActionFrom == null)
            {
                return true;
            }
            var resource = player[playerBoardAction.ActionFrom.ResourceType];

            switch (playerBoardAction.ActionFrom.ResourceKind)
            {
                case ResourceKind.Unit:
                    return resource.UnitCount + playerBoardAction.ActionFrom.Amount >= 0;

                case ResourceKind.Production:
                    if(resource.ResourceType == ResourceType.Money)
                    {
                        return resource.Production + playerBoardAction.ActionFrom.Amount >= -5;
                    }
                    return resource.Production + playerBoardAction.ActionFrom.Amount >= 0;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static async Task CanPlayCardAction(Board board, Player player)
        {
            await Task.CompletedTask;
            foreach (var playerAllPlayerAction in player.AllPlayerActions)
            {
                playerAllPlayerAction.CanExecute = CanPlayAction(player, playerAllPlayerAction);
            }
        }
    }
}