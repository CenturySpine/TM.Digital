using System.Collections.Generic;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Board
{
    public class GlobalParameterLevel
    {
        public BoardLevelType BoardLevelType { get; set; }
        public int Level { get; set; }
        public int Max { get; set; }
        public int Increment { get; set; }
        public int Min { get; set; }

        public List<BoardParameterThresold> BoardParameterThresolds { get; set; }
        public GlobalParameterLevel Clone()
        {
            return new GlobalParameterLevel
            {
                Min = Min,
                Max =  Max,
                Level = Level,
                Increment = Increment,
                BoardLevelType = BoardLevelType
            };
        }


    }


    public class BoardParameterThresold
    {
        public int Value { get; set; }

        public TileEffect TileEffect { get; set; }

        public ResourceEffect ResourceEffect { get; set; }

        public BoardLevelEffect BoardLevelEffect { get; set; }
    }
}