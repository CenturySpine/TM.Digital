using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Board
{
    public class GlobalParameterLevel
    {
        //public BoardLevelType BoardLevelType { get; set; }
        public int Level { get; set; }

        public bool IsFilled { get; set; }
        //public int Max { get; set; }
        //public int Increment { get; set; }
        //public int Min { get; set; }

        public List<BoardParameterThresold> BoardParameterThresolds { get; set; }
        public GlobalParameterLevel Clone()
        {
            return new GlobalParameterLevel
            {
                //Min = Min,
                //Max =  Max,
                BoardParameterThresolds = BoardParameterThresolds!=null?new List<BoardParameterThresold>(BoardParameterThresolds.Select(bp=>bp.Clone())):new List<BoardParameterThresold>(),
                IsFilled = IsFilled,
                Level = Level,
                //Increment = Increment,
                //BoardLevelType = BoardLevelType
            };
        }


        public override string ToString()
        {
            return $"{nameof(Level)}: {Level}";
        }
    }


    public class BoardParameterThresold
    {
        public TileEffect TileEffect { get; set; }

        public ResourceEffect ResourceEffect { get; set; }

        public BoardLevelEffect BoardLevelEffect { get; set; }

        public BoardParameterThresold Clone()
        {
            return new BoardParameterThresold()
            {
                TileEffect = TileEffect?.Clone(),ResourceEffect = ResourceEffect?.Clone(),BoardLevelEffect = BoardLevelEffect?.Clone()
            };
        }
    }
}