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
using TM.Digital.Model.Tile;
using TM.Digital.Services.Common;

namespace TM.Digital.Services
{

    public static class EffectHandler
    {

        private static int ComputeModifierValue(Player player, ResourceEffect effect, List<Player> allPlayers,
            Board board)
        {
            var mod = effect.EffectModifier;
            if (mod != null)
            {
                //List<Card> cards = new List<Card>();
                List<BoardPlace> places = new List<BoardPlace>();
                List<Tags> tags = new List<Tags>();
                switch (mod.ModifierFrom)
                {
                    case ActionTarget.Self:
                        //cards = player.AllPlayedCards;
                        places = BoardTilesHandler.GetPlayerTiles(board, player.PlayerId, mod.EffectModifierLocationConstraint);
                        tags = player.AllPlayedCards.SelectMany(c => c.Tags).ToList();
                        break;
                    case ActionTarget.CurrentCard:
                        break;
                    case ActionTarget.AnyOtherCard:
                        break;
                    case ActionTarget.AnyPlayer:
                        //var cards = allPlayers.SelectMany(p => p.AllPlayedCards).ToList();
                        places = allPlayers.Select(p => p.PlayerId).SelectMany(b => BoardTilesHandler.GetPlayerTiles(board, b, mod.EffectModifierLocationConstraint)).ToList();
                        tags = allPlayers.SelectMany(c => c.AllPlayedCards.SelectMany(p=>p.Tags)).ToList();
                        break;
                    case ActionTarget.AnyOpponent:
                       //var cards = allPlayers.Except(new List<Player> { player }).SelectMany(p => p.AllPlayedCards).ToList();
                        places = allPlayers.Except(new List<Player> { player }).Select(p => p.PlayerId).SelectMany(b => BoardTilesHandler.GetPlayerTiles(board, b, mod.EffectModifierLocationConstraint)).ToList();
                        tags = allPlayers.Except(new List<Player> { player }).SelectMany(c => c.AllPlayedCards.SelectMany(p => p.Tags)).ToList();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if(mod.TagsModifier != Tags.None)
                {
                    var tagNumber = tags.Count(t => t == mod.TagsModifier);
                    return (int)Math.Floor((double)tagNumber / mod.ModifierRatio);
                }
                if(mod.TileModifier != TileType.None)
                {
                    var tileNumber = places.Count(t => t.PlayedTile.Type == mod.TileModifier);
                    return (int)Math.Floor((double)tileNumber / mod.ModifierRatio);
                }
            }
            return 1;
        }
        public static async Task HandleResourceEffect(Player player, ResourceEffect effect, List<Player> allPlayers,
            Board board, CardDrawer cardDrawer)
        {

            
            if (effect.ResourceType == ResourceType.Card)
            {
                for (int i = 0; i < effect.Amount; i++)
                {
                    player.HandCards.Add(cardDrawer.DrawPatent());
                    await Logger.Log(player.Name, $"Drawing 1 card from deck");
                }
            }
            var resource = player.Resources.FirstOrDefault(r => r.ResourceType == effect.ResourceType);
            if (resource != null)
            {
                var modifierValue = ComputeModifierValue(player, effect, allPlayers, board);
                if (effect.ResourceKind == ResourceKind.Production)
                {

                    resource.Production += effect.Amount * modifierValue;

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
                    resource.UnitCount += effect.Amount * modifierValue;

                    if (resource.UnitCount < 0) resource.UnitCount = 0;
                    await Logger.Log(player.Name, $"Resource {resource.ResourceType} Unit modified for {effect.Amount}, new value {resource.UnitCount}");
                }
            }

        }

        public static async Task HandleInitialPatentBuy(Model.Player.Player player, List<Patent> selectionBoughtCards,
            Corporation selectionCorporation, Board board, List<Player> allPlayers, CardDrawer cardDrawer)
        {
            var playersMoney = player.Resources.First(r => r.ResourceType == ResourceType.Money);
            if (selectionCorporation != null)
            {
                foreach (var selectionCorporationResourcesEffect in selectionCorporation.ResourcesEffects)
                {
                    await HandleResourceEffect(player, selectionCorporationResourcesEffect, allPlayers, board, cardDrawer);
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

                playerHandCard.TitaniumUnitUsed = 0;
                playerHandCard.SteelUnitUsed = 0;
            }
        }
    }
}