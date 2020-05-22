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
        public static async Task HandleBoardEffect(BoardLevelEffect boardLevelEffect, Board board, Player player, CardDrawer cardDrawer)
        {
            switch (boardLevelEffect.BoardLevelType)
            {
                case BoardLevelType.Temperature:
                case BoardLevelType.Oxygen:
                    await Logger.Log(player.Name, $"Global parameter '{boardLevelEffect.BoardLevelType}' changed for {boardLevelEffect.Level}");


                    await IncreaseParameterLevel(board, boardLevelEffect.BoardLevelType, player, boardLevelEffect.Level, cardDrawer);

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

        public static async Task IncreaseParameterLevel(Board board, BoardLevelType parameter, Player playerObj, int incrementModifier, CardDrawer cardDrawer)
        {
            var temp = board.Parameters.First(p => p.Type == parameter);
            if (HasReachedMaxParam(board, parameter))
                return;

            //make it incremental to avoid bonuses greater than 1 goes beyond max and attribute unwanted NT points
            for (int i = 0; i < incrementModifier; i++)
            {
                var nextLevel = temp.Levels.Where(l => !l.IsFilled).Min(l => l.Level); //get next minimum non filled level
                var level = temp.Levels.FirstOrDefault(t => t.Level == nextLevel);
                //temp.GlobalParameterLevel.Level += temp.GlobalParameterLevel.Increment;
                if (level != null)
                {
                    level.IsFilled = true;
                    await Logger.Log(playerObj.Name, $"{parameter}: +{level.Level}");
                    foreach (var levelBoardParameterThresold in level.BoardParameterThresolds)
                    {


                        if (levelBoardParameterThresold?.BoardLevelEffect != null)
                        {
                            await HandleBoardEffect(levelBoardParameterThresold.BoardLevelEffect, board, playerObj, cardDrawer);
                        }

                        if (levelBoardParameterThresold?.ResourceEffect != null)
                        {
                            await EffectHandler.HandleResourceEffect(playerObj, levelBoardParameterThresold.ResourceEffect, new List<Player>(), board, cardDrawer, null);//ok for now because thresold are oly for current player, but might be extended
                        }

                        if (levelBoardParameterThresold?.TileEffect != null)
                        {
                            //TODO
                        }
                    }

                    //increase players NT
                    playerObj.TerraformationLevel += 1;

                    if (HasReachedMaxParam(board, parameter))
                        return;
                }
            }



        }


        public static bool HasReachedMaxParam(Board board, BoardLevelType ocean)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            var level = board.Parameters?.FirstOrDefault(p => p.Type == ocean)
                ?.Levels.All(l => l.IsFilled);


            return level.HasValue && level.Value;
        }

    }
}