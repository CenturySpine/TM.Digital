using System;
using System.Collections.Generic;

namespace TM.Digital.Model.Board
{
    public class Board
    {
        public List<BoardLine> BoardLines { get; set; }

        public List<BoardPlace> IsolatedPlaces { get; set; }

        public List<BoardParameter> Parameters { get; set; } = new List<BoardParameter>()
        {
            new BoardParameter
            {
               Type = BoardLevelType.Temperature,
                GlobalParameterLevel = new GlobalParameterLevel {BoardLevelType = BoardLevelType.Temperature,Level = -30,Min=-30,Increment = 2, Max = 8}

            },
            new BoardParameter()
            {
                Type = BoardLevelType.Oxygen,
                GlobalParameterLevel = new GlobalParameterLevel {BoardLevelType = BoardLevelType.Oxygen,Level = 0, Min=0,Increment = 1, Max = 14}

            },
            new BoardParameter()
            {
                Type = BoardLevelType.Oceans,
                GlobalParameterLevel = new GlobalParameterLevel {BoardLevelType = BoardLevelType.Oceans,Level = 0,Min=0,Increment = 1, Max = 9}

            },
        };



    }
}