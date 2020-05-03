using System;
using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    public class GlobalCheckPrerequisite : IPrerequisiteStrategy
    {
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            if (inputPatent.Prerequisites?.GlobalPrerequisites != null)
                foreach (var patentPrerequisite in inputPatent.Prerequisites.GlobalPrerequisites)
                {
                    var boardParam = currentBoardState.Parameters.First(p => p.Type == patentPrerequisite.Parameter);

                    var filledLevels = boardParam.Levels.Where(l => l.IsFilled).ToList();

                    if (patentPrerequisite.IsMax)
                    {
                        if (!filledLevels.Any())
                            return true;

                        return !(filledLevels.Max(l => l.Level) > patentPrerequisite.Value);
                    }

                    if (!filledLevels.Any())
                        return false;


                    return filledLevels.Max(l => l.Level) >= patentPrerequisite.Value;
                }

            return true;
        }


    }
}