using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Client.Services
{
    public interface IApiProxy
    {
        Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName);

        Task<GameSessions> GetGameSessions();

        Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers, string selectedBoardName);

        Task<bool> StartGame(Guid selectedSessionGameSessionId);
        Task<Player> SendSetup(GameSetupSelection gSetup);
        Task PlaceTile(BoardPlace bp);
        Task<Board> GetBoard(string boardName);
        Task Skip();
        Task Pass();
        Task PlayBoardAction(BoardAction boardAction);
        Task PlayCardAction(Action cardAction);
        Task PlayConvertResourcesAction(ResourceHandler rh);
        Task PlayCard(CardActionPlay cardAction);
        Task VerifyCardPlayability(PatentSelector patent);
        Task VerifyResourcesModifiedCostCardPlayability(PlayCardWithResources mod);
        Task SelectEffectTarget(ResourceEffectPlayerChooser choice);
        Task<List<Board>> GetBoards();
        Task<bool> EnsureInit();
    }
}