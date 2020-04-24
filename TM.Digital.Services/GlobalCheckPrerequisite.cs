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
        Dictionary<BoardLevelType,Func<GlobalPrerequisite,Board, bool>> _strategies = new Dictionary<BoardLevelType, Func<GlobalPrerequisite, Board, bool>>();
        public bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner)
        {
            if (inputPatent.Prerequisites?.GlobalPrerequisites != null)
                foreach (var patentPrerequisite in inputPatent.Prerequisites.GlobalPrerequisites)
                {
                    var boardParam = currentBoardState.Parameters.First(p => p.Type == patentPrerequisite.Parameter);

                    if (patentPrerequisite.IsMax)
                    {
                        if (boardParam.GlobalParameterLevel.Level > patentPrerequisite.Value)
                            return false;
                        return true;
                    }

                    if (boardParam.GlobalParameterLevel.Level < patentPrerequisite.Value)
                        return false;
                }

            return true;
        }


    }
}