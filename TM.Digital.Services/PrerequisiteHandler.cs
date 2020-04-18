using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
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
        static readonly List<IPrerequisiteStrategy> VerifyStrategies = new List<IPrerequisiteStrategy>()
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



    }

    public class TilePlacementRequirements : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            if (inputPatent.TileEffects == null || inputPatent.TileEffects.Count == 0) return true;
            var nonOccupiedSpaces = BoardHandler.GetNonOccupiedSpaces(currentBoardState);
            if (nonOccupiedSpaces.Count == 0) return false;
            foreach (var patentTileEffect in inputPatent.TileEffects)
            {
                switch (patentTileEffect.Constrains)
                {
                    case TilePlacementCosntrains.None:
                        return BoardHandler.AnyNonReservedSpace(nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.ReservedForOcean:
                        return BoardHandler.GetOceansSpaces(nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.StandardCity:
                        return BoardHandler.GetCitySpaces(currentBoardState, nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.VolcanicSpace:
                        return BoardHandler.VolcanicSpaces(nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.NothingAround:
                        return BoardHandler.SpacesWithNothingAround(nonOccupiedSpaces, currentBoardState).Count > 0;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Check that the player owns the required number of target tags among its played cards, NOT INCLUDING EVENT/RED CARDS
    /// </summary>
    public class TagsCountPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            var playerTagsCount = patentOwner.PlayedCards.Concat(new List<Card>{ patentOwner.Corporation }) // players cards + corporation
                .Where(c => c.CardType != CardType.Red)//different from event/red
                .SelectMany(c => c.Tags)//Select ALL tags
                .GroupBy(r => r)//group selection by tag type
                .Select(grp => new { Tag = grp.Key, Count = grp.Count() })//returns object [tag type,count]
                .ToList();

            //for each patent tag prerequisite
            foreach (var patentTagsPrerequisite in inputPatent.Prerequisites.TagsPrerequisites)
            {
                //get the players number of matching tags
                var playerTagsMatch = playerTagsCount.FirstOrDefault(t => t.Tag == patentTagsPrerequisite.Tag);

                //must be a least equal to number prerequisite
                if (playerTagsMatch == null || playerTagsMatch.Count < patentTagsPrerequisite.Value)
                    return false;
            }

            return true;
        }
    }
    /// <summary>
    /// Check that if the card requires to reduce player's own resource production, the player has enough production in this resource
    /// </summary>
    public class ResourcesProductionReductionPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            var resourcesUnitCost = inputPatent.ResourcesEffects
                .Where(r => r.ResourceKind == ResourceKind.Production
                            && r.EffectDestination == ActionTarget.Self && r.Amount < 0)
                .ToList();

            foreach (var resourceCost in resourcesUnitCost)
            {
                var playerResource = patentOwner.Resources.First((r => r.ResourceType == resourceCost.ResourceType));
                if (playerResource.Production < Math.Abs(resourceCost.Amount))
                {
                    return false;
                }
            }

            return true;
        }
    }

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
    public class GlobalCheckPrerequisite : IPrerequisiteStrategy
    {
        Dictionary<BoardLevelType,Func<GlobalPrerequisite,Board, bool>> _strategies = new Dictionary<BoardLevelType, Func<GlobalPrerequisite, Board, bool>>();
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            foreach (var patentPrerequisite in inputPatent.Prerequisites.GlobalPrerequisites)
            {
                var boardParam = currentBoardState.Parameters.First(p => p.Type == patentPrerequisite.Parameter);

                if (patentPrerequisite.IsMax)
                {
                    if(boardParam.GlobalParameterLevel.Level > patentPrerequisite.Value)
                        return false;
                    return true;
                }
                    
                if (boardParam.GlobalParameterLevel.Level< patentPrerequisite.Value)
                    return false;
            }

            return true;
        }


    }
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
    public interface IPrerequisiteStrategy

    {
        bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner);
    }
}