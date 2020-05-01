using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Services.Common;

namespace TM.Digital.Services
{
    public static class BoardEffectHandler
    {
        public static async Task HandleBoardEffect(BoardLevelEffect boardLevelEffect, Board board, Player player)
        {
            switch (boardLevelEffect.BoardLevelType)
            {
                case BoardLevelType.Temperature:
                case BoardLevelType.Oxygen:
                    await Logger.Log(player.Name, $"Global parameter '{boardLevelEffect.BoardLevelType}' changed for {boardLevelEffect.Level}");


                    await IncreaseParameterLevel(board, boardLevelEffect.BoardLevelType, player, boardLevelEffect.Level);

                    //var parameter = board.Parameters.First(b => b.Type == boardLevelEffect.BoardLevelType);
                    //parameter.GlobalParameterLevel.Level += boardLevelEffect.Level * ((boardLevelEffect.BoardLevelType == BoardLevelType.Temperature) ? 2 : 1);
                    //await  Logger.Log(player.Name, $"Increasing '{player.Name}' terraformation level by {boardLevelEffect.Level}");
                    //player.TerraformationLevel += boardLevelEffect.Level;

                    break;
                case BoardLevelType.Terraformation:
                    player.TerraformationLevel += boardLevelEffect.Level;
                    await Logger.Log(player.Name, $"Increasing '{player.Name}' terraformation level by {boardLevelEffect.Level}");
                    break;
                case BoardLevelType.Oceans:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public static async Task IncreaseParameterLevel(Board board, BoardLevelType parameter, Player playerObj, int incrementModifier)
        {
            var temp = board.Parameters.First(p => p.Type == parameter);
            if (HasReachedMaxParam(board, parameter))
                return;

            //make it incremental to avoid bonuses greater than 1 goes beyond max and attribute unwanted NT points
            for (int i = 0; i < incrementModifier; i++)
            {
                temp.GlobalParameterLevel.Level += temp.GlobalParameterLevel.Increment;
                await Logger.Log(playerObj.Name, $"{parameter}: +{temp.GlobalParameterLevel.Increment}");
                var thresold =
                    temp.GlobalParameterLevel.BoardParameterThresolds.FirstOrDefault(t =>
                        t.Value == temp.GlobalParameterLevel.Level);

                if (thresold?.BoardLevelEffect != null)
                {
                    await HandleBoardEffect(thresold.BoardLevelEffect, board, playerObj);
                }

                if (thresold?.ResourceEffect != null)
                {
                    await EffectHandler.HandleResourceEffect(playerObj, thresold.ResourceEffect, new List<Player>(), board);//ok for now because thresold are oly for current player, but might be extended
                }

                if (thresold?.TileEffect != null)
                {
                    //TODO
                }
                //increase players NT
                playerObj.TerraformationLevel += 1;

                if (HasReachedMaxParam(board, parameter))
                    return;

            }



        }


        public static bool HasReachedMaxParam(Board board, BoardLevelType ocean)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            var level = board.Parameters?.FirstOrDefault(p => p.Type == ocean)
                ?.GlobalParameterLevel;
            if (level != null && level.Level == level.Max)
                return true;

            return false;
        }

    }
}