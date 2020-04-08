using System;
using System.Threading.Tasks;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;

namespace TM.Digital.Client.Services
{
    public interface IApiProxy
    {
        Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName);

        Task<GameSessions> GetGameSessions();

        Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers);

        Task<bool> StartGame(Guid sessionGameSessionId);
    }
}