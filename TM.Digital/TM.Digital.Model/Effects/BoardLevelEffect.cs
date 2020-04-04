using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Effects
{
    public class BoardLevelEffect : IEffect
    {
        public BoardLevelType BoardLevelType { get; set; }
        public int Level { get; set; }

        

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            //board.GlobalParameterLevels[BoardLevelType].Level +=
            //    Level * board.GlobalParameterLevels[BoardLevelType].Increment;
        }
    }
}