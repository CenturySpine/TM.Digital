namespace TM.Digital.Model.Cards
{
    public interface IAction
    {
        void Execute(Board.Board board, Player.Player player);
    }
}