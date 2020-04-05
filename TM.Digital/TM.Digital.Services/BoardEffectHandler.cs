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
                    Logger.Log(player.Name, $"Global parameter '{boardLevelEffect.BoardLevelType}' changed for {boardLevelEffect.Level}");

                    var parameter = board.Parameters.First(b => b.Type == boardLevelEffect.BoardLevelType);
                    parameter.GlobalParameterLevel.Level += boardLevelEffect.Level * ((boardLevelEffect.BoardLevelType == BoardLevelType.Temperature) ? 2 : 1);
                    Logger.Log(player.Name, $"Increasing '{player.Name}' terraformation level by {boardLevelEffect.Level}");
                    player.TerraformationLevel += boardLevelEffect.Level;

                    break;
                case BoardLevelType.Terraformation:
                    player.TerraformationLevel += boardLevelEffect.Level;
                    Logger.Log(player.Name, $"Increasing '{player.Name}' terraformation level by {boardLevelEffect.Level}");
                    break;
                case BoardLevelType.Oceans:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}