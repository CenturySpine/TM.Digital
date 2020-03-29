namespace TM.Digital.Model.Prerequisite
{
    public interface IPrerequisite
    {
        bool MatchPrerequisite(Player.Player player, Board.Board board);

    }
}