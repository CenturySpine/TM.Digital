using System;
using System.Threading.Tasks;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;

namespace TM.Digital.Client.Services
{
    public class ApiProxy : IApiProxy
    {
        public async Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<Player>($"game/join/{selectedSessionGameSessionId}/{playerName}");
        }

        public async Task<GameSessions> GetGameSessions()
        {
            return await TmDigitalClientRequestHandler.Instance.Request<GameSessions>("game/sessions");
        }

        public async Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<GameSessionInformation>($"game/create/{playerName}/{numberOfPlayers}");
        }

        public async Task<bool> StartGame(Guid sessionGameSessionId)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<bool>($"game/start/{sessionGameSessionId}");
        }
    }
}