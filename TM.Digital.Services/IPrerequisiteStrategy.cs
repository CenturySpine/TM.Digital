using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;

namespace TM.Digital.Services
{
    public interface IPrerequisiteStrategy

    {
        bool CanPlayCard(Patent inputPatent, Board currentBoardState, Player patentOwner);
    }
}