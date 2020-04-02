using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Effects
{
    public class GlobalParameterLevelEffect : IEffect
    {
        public GlobalParameterType GlobalParameterType { get; set; }
        public int Level { get; set; }

        

        public void Apply(Player.Player player, Board.Board board, Card card)
        {
            board.GlobalParameterLevels[GlobalParameterType].Level +=
                Level * board.GlobalParameterLevels[GlobalParameterType].Increment;
        }
    }
}