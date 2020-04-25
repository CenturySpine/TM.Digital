namespace TM.Digital.Model.Board
{
    public class BoardParameter
    {
        public BoardLevelType Type { get; set; }
        public GlobalParameterLevel GlobalParameterLevel { get; set; }

        public BoardParameter Clone()
        {
            return new BoardParameter
            {
                GlobalParameterLevel = GlobalParameterLevel.Clone(),
                Type = Type,
            };
        }
    }
}