using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Services.Common;
using TM.Digital.Transport.Hubs;

namespace TM.Digital.Services
{
    public static class BoardHandler
    {
        public static List<BoardPlace> SpacesWithNothingAround(List<BoardPlace> nonOccupiedSpaces, Model.Board.Board board)
        {
            List<BoardPlace> availablePlaces = new List<BoardPlace>();
            //among available spaces, select those which are not exclusively reserved
            var filteredPlaces = nonOccupiedSpaces.Where(p => !p.Reserved.IsExclusive);
            foreach (var space in filteredPlaces)
            {
                //get all their surrounding spaces
                var surroundingIndexes = GetPlaceSurroundingsIndexes(space, board);
                var surroundingTiles = board.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => surroundingIndexes.Contains(p.Index)).ToList();
                if (surroundingTiles.Any(t => t.PlayedTile != null))
                {
                    continue;
                }
                availablePlaces.Add(space);
            }

            return availablePlaces;
        }

        public static List<BoardPlace> GetCitySpaces(Model.Board.Board board, List<BoardPlace> nonOccupiedSpaces)
        {
            List<BoardPlace> availablePlaces = new List<BoardPlace>();
            //among available spaces, select those which are not exclusively reserved
            var filteredPlaces = nonOccupiedSpaces.Where(p => !p.Reserved.IsExclusive);
            foreach (var space in filteredPlaces)
            {
                //get all their surrounding spaces
                var surroundingIndexes = GetPlaceSurroundingsIndexes(space, board);
                var surroundingTiles = board.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => surroundingIndexes.Contains(p.Index)).ToList();
                if (surroundingTiles.Any(t => t.PlayedTile != null && (t.PlayedTile.Type == TileType.City || t.PlayedTile.Type == TileType.Capital)))
                {
                    continue;
                }
                availablePlaces.Add(space);
            }

            return availablePlaces;
        }

        private static PlaceCoordinates[] GetPlaceSurroundingsIndexes(BoardPlace space, Board board)
        {

            var currentLine = board.BoardLines.FirstOrDefault(l => l.BoardPlaces.FirstOrDefault(p => p.Index.Equals(space.Index)) != null);
            //int linePlaceCount = currentLine.BoardPlaces.Count;
            //int upperLinePlaceCount = currentLine.Index > 0 ? board.BoardLines[currentLine.Index - 1].BoardPlaces.Count() : 0;
            //int lowerLinePlaceCount = currentLine.Index < board.BoardLines.Count ? board.BoardLines[currentLine.Index + 1].BoardPlaces.Count() : 0;

            var surr = new PlaceCoordinates[]
            {
                //upper left surroundings
                new PlaceCoordinates
                {
                    X = space.Index.X - 1,
                    Y = space.Index.Y -1
                },

                //middle left surroundings
                new PlaceCoordinates
                {
                        X = space.Index.X ,
                        Y = space.Index.Y -1
                },

                //bottom left surroundings
                new PlaceCoordinates
                {
                    X = space.Index.X+1 ,
                    Y = space.Index.Y -1
                },

                //upper right surroundings
                new PlaceCoordinates
                {
                    X = space.Index.X-1 ,
                    Y = space.Index.Y 
                },

                //middle right surroundings
                new PlaceCoordinates
                {
                    X = space.Index.X ,
                    Y = space.Index.Y+1
                },
                //bottom right surroundings
                new PlaceCoordinates
                {
                    X = space.Index.X+1 ,
                    Y = space.Index.Y 
                },


            };
            return surr;
        }

        public static List<BoardPlace> GetNonOccupiedSpaces(Model.Board.Board board)
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

        public static Model.Board.Board GetPlacesChoices(TileEffect first, Model.Board.Board board)
        {
            List<BoardPlace> choicePlaces;
            var nonOccupiedSpaces = BoardHandler.GetNonOccupiedSpaces(board);
            switch (first.Constrains)
            {
                case TilePlacementCosntrains.None:
                    choicePlaces = BoardHandler.AnyNonReservedSpace(nonOccupiedSpaces);
                    break;

                case TilePlacementCosntrains.ReservedForOcean:
                    choicePlaces = BoardHandler.GetOceansSpaces(nonOccupiedSpaces);
                    break;

                case TilePlacementCosntrains.StandardCity:
                    choicePlaces = BoardHandler.GetCitySpaces(board, nonOccupiedSpaces);
                    break;

                case TilePlacementCosntrains.VolcanicSpace:
                    choicePlaces = BoardHandler.VolcanicSpaces(nonOccupiedSpaces);
                    break;

                case TilePlacementCosntrains.NothingAround:
                    choicePlaces = BoardHandler.SpacesWithNothingAround(nonOccupiedSpaces, board);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var choicePlace in choicePlaces)
            {
                choicePlace.CanBeChosed = true;
            }

            return board;
        }

        public static async Task PlaceTileOnBoard(BoardPlace place, Model.Player.Player playerId, TileEffect pendingTileEffect,
            Model.Board.Board board, CardDrawer cardDrawer)
        {
            var allPlaces = board.BoardLines.SelectMany(l => l.BoardPlaces).ToList();
            var targetTile = allPlaces.FirstOrDefault(p => p.Index.Equals(place.Index));
            if (targetTile != null)
            {
                targetTile.PlayedTile = new Tile
                {
                    Owner = pendingTileEffect.Type != TileType.Ocean ? playerId.PlayerId : default(Guid?),
                    Type = pendingTileEffect.Type,
                };

                //inherent tile placement bonus
                if (targetTile.PlacementBonus.Any())
                {
                    await Logger.Log(playerId.Name, $"Evaluating tile placement bonuses...");

                    var bonusGroup = targetTile.PlacementBonus.GroupBy(b => b.BonusType);
                    foreach (var bonus in bonusGroup)
                    {
                        var resource = playerId.Resources.FirstOrDefault(t => t.ResourceType == bonus.Key);
                        if (resource != null)
                        {
                            resource.UnitCount += bonus.Count();
                            await Logger.Log(playerId.Name, $"Resource '{resource.ResourceType}' bonus of {bonus.Count()} units");
                        }
                        else
                        {
                            if (bonus.Key == ResourceType.Card)
                            {
                                for (int i = 0; i < bonus.Count(); i++)
                                {
                                    playerId.HandCards.Add(cardDrawer.DrawPatent());
                                    await Logger.Log(playerId.Name, $"Drawing 1 card from deck");
                                }
                            }
                        }
                    }
                }

                //bonus for placement near ocean tiles
                await Logger.Log(playerId.Name, $"Evaluating ocean tile placement bonuses...");
                var surroundings = GetPlaceSurroundingsIndexes(place, board);
                var surroundingTiles = board.BoardLines.SelectMany(r => r.BoardPlaces).Where(p => surroundings.Contains(p.Index)).ToList();
                var playedOcenTiles = surroundingTiles
                    .Where(t => t.PlayedTile != null && t.PlayedTile.Type == TileType.Ocean).ToList();
                await Logger.Log(playerId.Name, $"Found {playedOcenTiles.Count} ocean tiles... {string.Join(',',playedOcenTiles.Select(t=>t.Index.ToString()))}");
                var playerResource = playerId.Resources.First(r => r.ResourceType == ResourceType.Money);
                playerResource.UnitCount += playedOcenTiles.Count * 2;
                await Logger.Log(playerId.Name, $"Money units increased by {playedOcenTiles.Count * 2}");

                //terraformation bonuses
                if (pendingTileEffect.Type == TileType.Ocean)
                {
                    await Logger.Log(playerId.Name, $"Ocean placed, increasing global parameter and player's terraformation level");
                    board.Parameters.First(p => p.Type == BoardLevelType.Oceans).GlobalParameterLevel.Level += 1;
                    playerId.TerraformationLevel += 1;
                }
                if (pendingTileEffect.Type == TileType.Forest)
                {
                    await Logger.Log(playerId.Name, $"Forest placed, increasing global parameter and player's terraformation level");
                    board.Parameters.First(p => p.Type == BoardLevelType.Oxygen).GlobalParameterLevel.Level += 1;
                    playerId.TerraformationLevel += 1;
                }

                allPlaces.ForEach(p => p.CanBeChosed = false);
            }
        }
    }
}