using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services.Common;

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

        private static readonly List<IPrerequisiteStrategy> VerifyStrategies = new List<IPrerequisiteStrategy>()
        {
            new GlobalCheckPrerequisite(),
            new PatentCostPrerequisite(),
            new ResourcesProductionReductionPrerequisite(),
            new ResourcesUnitReductionPrerequisite(),
            new TagsCountPrerequisite(),
            new TilePlacementRequirements()
        };

        private static bool CanPlayCard(Patent patent, Model.Board.Board board, Model.Player.Player player)
        {
            return VerifyStrategies.All(t => t.CanPlayCard(patent, board, player));
        }

        public static async Task CanConvertResources(Board board, Player player)
        {
            await Task.CompletedTask;

            var plantConversion = GetPlantConversionRate(player);
            var heatConversion = GetHeatConversionRate(player);
            var plants = player[ResourceType.Plant];
            if (plants.UnitCount >= plantConversion)
            {
                plants.CanConvert = true;
            }

            var heat = player[ResourceType.Heat];
            if (heat.UnitCount >= heatConversion)
            {
                heat.CanConvert = true;
            }
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
            var cr = player.PlayedCards.Concat(new List<Card>(){ player.Corporation } ).Where(p => p.ConversionRates != null)
                .Select(pc => pc.ConversionRates).ToList();

            var plantConversion = cr.Any() ?
                cr.Where(p => p.PlantConversion != null)
                    .Select(p => p.PlantConversion)
                    .Min(t => t.Rate)
                : 0;

            if (plantConversion == 0) plantConversion = 8;
            return plantConversion;
        }
    }
}