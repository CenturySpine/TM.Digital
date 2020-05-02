using System.Collections.Generic;
using System.Linq;

namespace TM.Digital.Model.Board
{
    public class BoardParameter
    {
        public BoardLevelType Type { get; set; }
        //public GlobalParameterLevel GlobalParameterLevel { get; set; }

        public List<GlobalParameterLevel> Levels { get; set; }
        public BoardParameter Clone()
        {
            return new BoardParameter
            {
                //GlobalParameterLevel = GlobalParameterLevel.Clone(),
                Levels = new List<GlobalParameterLevel>(Levels.Select(l=>l.Clone())),
                Type = Type,
            };
        }
    }
}