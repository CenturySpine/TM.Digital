using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Effects
{
    public static class BoardHandler
    {
        public static List<BoardPlace> SpacesWithNothingAround(List<BoardPlace> nonOccupiedSpaces, Board.Board board)
        {
            List<BoardPlace> availablePlaces = new List<BoardPlace>();
            //among available spaces, select those which are not exclusively reserved
            var filteredPlaces = nonOccupiedSpaces.Where(p => !p.Reserved.IsExclusive);
            foreach (var space in filteredPlaces)
            {
                //get all their surrounding spaces
                var surroundingIndexes = GetPlaceSurroundingsIndexes(space);
                var surroundingTiles = board.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => surroundingIndexes.Contains(p.Index)).ToList();
                if (surroundingTiles.Any(t => t.PlayedTile != null))
                {
                    continue;
                }
                availablePlaces.Add(space);
            }

            return availablePlaces;
        }

        public static List<BoardPlace> GetCitySpaces(Board.Board board, List<BoardPlace> nonOccupiedSpaces)
        {
            List<BoardPlace> availablePlaces = new List<BoardPlace>();
            //among available spaces, select those which are not exclusively reserved
            var filteredPlaces = nonOccupiedSpaces.Where(p => !p.Reserved.IsExclusive);
            foreach (var space in filteredPlaces)
            {
                //get all their surrounding spaces
                var surroundingIndexes = GetPlaceSurroundingsIndexes(space);
                var surroundingTiles = board.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => surroundingIndexes.Contains(p.Index)).ToList();
                if (surroundingTiles.Any(t => t.PlayedTile != null && (t.PlayedTile.Type == TileType.City || t.PlayedTile.Type == TileType.Capital)))
                {
                    continue;
                }
                availablePlaces.Add(space);
            }

            return availablePlaces;
        }

        private static Point[] GetPlaceSurroundingsIndexes(BoardPlace space)
        {
            var surr = new Point[]
            {
                //left surroundings
                new Point(space.Index.X - 1, space.Index.Y - 1),
                new Point(space.Index.X, space.Index.Y - 1),
                new Point(space.Index.X + 1, space.Index.Y - 1),

                //right surroundings
                new Point(space.Index.X - 1, space.Index.Y),
                new Point(space.Index.X, space.Index.Y + 1),
                new Point(space.Index.X + 1, space.Index.Y),
            };
            return surr;
        }

        public static List<BoardPlace> GetNonOccupiedSpaces(Board.Board board)
        {
            return board.BoardLines.SelectMany(l => l.BoardPlaces).Where(p => p.PlayedTile == null).ToList();
        }

        public static List<BoardPlace> GetOceansSpaces(List<BoardPlace> board)
        {
            return board.Where(t => t.Reserved.ReservedFor == ReservedFor.Ocean && t.Reserved.IsExclusive).ToList();
        }

        public static List<BoardPlace> AnyNonReservedSpace(List<BoardPlace> nonOccupiedSpaces)
        {
            return nonOccupiedSpaces.Where(t => !t.Reserved.IsExclusive).ToList();
        }

        public static List<BoardPlace> VolcanicSpaces(List<BoardPlace> nonOccupiedSpaces)
        {
            return nonOccupiedSpaces.Where(t => t.Reserved.ReservedFor == ReservedFor.Volcano).ToList();
        }
    }

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
                   && CheckTags(player, patent)
            && CheckTilePlacement(board, patent, player);
        }

        private static bool CheckTilePlacement(Board.Board board, Patent patent, Player.Player player)
        {
            if (patent.TileEffects == null || patent.TileEffects.Count == 0) return true;
            var nonOccupiedSpaces = BoardHandler.GetNonOccupiedSpaces(board);
            if (nonOccupiedSpaces.Count == 0) return false;
            foreach (var patentTileEffect in patent.TileEffects)
            {
                switch (patentTileEffect.Constrains)
                {
                    case TilePlacementCosntrains.None:
                        return BoardHandler.AnyNonReservedSpace(nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.ReservedForOcean:
                        return BoardHandler.GetOceansSpaces(nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.StandardCity:
                        return BoardHandler.GetCitySpaces(board, nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.VolcanicSpace:
                        return BoardHandler.VolcanicSpaces(nonOccupiedSpaces).Count > 0;

                    case TilePlacementCosntrains.NothingAround:
                        return BoardHandler.SpacesWithNothingAround(nonOccupiedSpaces, board).Count > 0;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return false;
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

            var allCards = player.PlayedCards.Concat(new List<Card> { player.Corporation }).ToList();

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