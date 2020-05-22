using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services.Common;
using TM.Digital.Services.ResourcesHandlers;

namespace TM.Digital.Services
{
    public static class EffectHandler
    {
        private static readonly SelfResourceEffectHandler SelfHandler = new SelfResourceEffectHandler();
        private static readonly AnyPlayerResourceEffectHandler AnyPlayerHandler = new AnyPlayerResourceEffectHandler();
        private static readonly TargetCardHandler TargetCardHandler = new TargetCardHandler();
        private static readonly OtherCardHandler OtherCardHandler = new OtherCardHandler();
        public static async Task<Action<Player, Board>> HandleResourceEffect(Player player, ResourceEffect effect,
            List<Player> allPlayers,
            Board board, CardDrawer cardDrawer, Patent card)
        {
            if (effect.EffectDestination == ActionTarget.Self)
            {
                await SelfHandler.Resolve(player, effect, allPlayers, board, cardDrawer);
                return null;
            }

            if (effect.EffectDestination == ActionTarget.AnyPlayer)
            {
                return AnyPlayerHandler.Resolve(effect, allPlayers);
            }

            if (effect.EffectDestination == ActionTarget.ToCurrentCard)
            {
                await TargetCardHandler.Resolve(player, effect, allPlayers, board, card);
            }
            if (effect.EffectDestination == ActionTarget.ToAnyOtherCard)
            {
                return OtherCardHandler.Resolve(player, effect, allPlayers, board, card);
            }
            return null;
        }

        public static async Task HandleInitialPatentBuy(Model.Player.Player player, List<Patent> selectionBoughtCards,
            Corporation selectionCorporation, Board board, List<Player> allPlayers, CardDrawer cardDrawer)
        {
            var playersMoney = player.Resources.First(r => r.ResourceType == ResourceType.Money);
            if (selectionCorporation != null)
            {
                foreach (var selectionCorporationResourcesEffect in selectionCorporation.ResourcesEffects)
                {
                    await HandleResourceEffect(player, selectionCorporationResourcesEffect, allPlayers, board, cardDrawer, null);
                }
                await Logger.Log(player.Name, $"Initial money count {playersMoney.UnitCount}");
            }

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
            var allResourcesModifier = player.PlayedCards.Concat(new List<Card> { player.Corporation }).Where(r => r.MineralModifiers != null).SelectMany(r => r.MineralModifiers).ToList();

            foreach (var modifier in allResourcesModifier.GroupBy(r => r.ResourceType))
            {
                var handler = player.Resources.First(r => r.ResourceType == modifier.Key);
                if (handler != null)
                {
                    int initialValue = 0;
                    switch (modifier.Key)
                    {
                        case ResourceType.None:
                            break;
                        case ResourceType.Steel:
                            initialValue = GameConstants.SteelValue;
                            break;
                        case ResourceType.Titanium:
                            initialValue = GameConstants.TitaniumValue;
                            break;
                    }

                    handler.MoneyValueModifier = initialValue + modifier.Sum(r => r.Value);
                }

            }

            //force steel and titanium to their base value
            if (player[ResourceType.Steel].MoneyValueModifier < GameConstants.SteelValue)
                player[ResourceType.Steel].MoneyValueModifier = GameConstants.SteelValue;

            if (player[ResourceType.Titanium].MoneyValueModifier < GameConstants.TitaniumValue)
                player[ResourceType.Titanium].MoneyValueModifier = GameConstants.TitaniumValue;
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

                playerHandCard.TitaniumUnitUsed = 0;
                playerHandCard.SteelUnitUsed = 0;
            }
        }
    }
}