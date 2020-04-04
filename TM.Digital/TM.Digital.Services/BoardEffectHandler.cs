using System;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    public static class BoardEffectHandler
    {
        public static void HandleBoardEffect(BoardLevelEffect boardLevelEffect, Board board, Player player)
        {
            switch (boardLevelEffect.BoardLevelType)
            {
                case BoardLevelType.Temperature:
                case BoardLevelType.Oxygen:
                    board.Parameters.First(b => b.Type == boardLevelEffect.BoardLevelType).GlobalParameterLevel.Level +=
                        boardLevelEffect.Level;
                    player.TerraformationLevel += boardLevelEffect.Level;
                    break;
                case BoardLevelType.Terraformation:
                    player.TerraformationLevel += boardLevelEffect.Level;

                    break;
                case BoardLevelType.Oceans:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}