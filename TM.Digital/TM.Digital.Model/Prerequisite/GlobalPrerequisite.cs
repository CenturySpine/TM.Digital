using System;
using TM.Digital.Model.Board;

namespace TM.Digital.Model.Prerequisite
{
    public class GlobalPrerequisite : IPrerequisite
    {
        public GlobalParameterType Parameter { get; set; }
        public int Level { get; set; }
        public bool IsMax { get; set; }

        public bool MatchPrerequisite(Player.Player player, Board.Board board)
        {
            switch (Parameter)
            {
                case GlobalParameterType.Heat:
                    return IsMax
                        ? board.GlobalParameterLevels[Parameter].Level <= Level
                        : board.GlobalParameterLevels[Parameter].Level >= Level;
                case GlobalParameterType.Oxygen:
                    return IsMax
                        ? board.GlobalParameterLevels[Parameter].Level <= Level
                        : board.GlobalParameterLevels[Parameter].Level >= Level;
                case GlobalParameterType.Oceans:
                    return IsMax
                        ? board.GlobalParameterLevels[Parameter].Level >= Level
                        : board.GlobalParameterLevels[Parameter].Level <= Level;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}