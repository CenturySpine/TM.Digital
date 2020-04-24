using System;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Tile;

namespace TM.Digital.Services
{
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
}