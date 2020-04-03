using System.Collections.Generic;

namespace TM.Digital.Model.Board
{
    public class Board
    {
        public List<BoardLine> BoardLines { get; set; }

        public List<BoardPlace> IsolatedPlaces { get; set; }

        //public Dictionary<GlobalParameterType, GlobalParameterLevel> GlobalParameterLevels { get; set; } = new Dictionary<GlobalParameterType, GlobalParameterLevel>
        //{
        //    {GlobalParameterType.Temperature, new GlobalParameterLevel {GlobalParameterType = GlobalParameterType.Temperature,Level = -30,Increment = 2, Max = 8}},
        //    {GlobalParameterType.Oxygen,new GlobalParameterLevel {GlobalParameterType = GlobalParameterType.Oxygen,Level = 0, Increment = 1, Max = 14}},
        //    { GlobalParameterType.Oceans,  new GlobalParameterLevel {GlobalParameterType = GlobalParameterType.Oceans,Level = 9,Increment = -1, Max = 0}},
        //};


    }
}