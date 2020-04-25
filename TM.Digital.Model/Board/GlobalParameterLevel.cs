namespace TM.Digital.Model.Board
{
    public class GlobalParameterLevel
    {
        public BoardLevelType BoardLevelType { get; set; }
        public int Level { get; set; }
        public int Max { get; set; }
        public int Increment { get; set; }
        public int Min { get; set; }

        public GlobalParameterLevel Clone()
        {
            return new GlobalParameterLevel()
            {
                Min = Min,
                Max =  Max,
                Level = Level,
                Increment = Increment,
                BoardLevelType = BoardLevelType
            };
        }
    }
}