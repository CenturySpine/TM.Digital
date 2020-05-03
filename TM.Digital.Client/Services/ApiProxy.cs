using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Client.Services
{
    public class ApiProxy : IApiProxy
    {
        public async Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers,
            string selectedBoardName)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<GameSessionInformation>($"game/create/{playerName}/{numberOfPlayers}/{selectedBoardName}");
        }

        public async Task<Board> GetBoard(string boardName)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<Board>($"marsboard/{boardName}");
        }

        public async Task<GameSessions> GetGameSessions()
        {
            return await TmDigitalClientRequestHandler.Instance.Request<GameSessions>("game/sessions");
        }

        public async Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<Player>($"game/join/{selectedSessionGameSessionId}/{playerName}");
        }

        public async Task Pass()
        {
            await TmDigitalClientRequestHandler.Instance.Request<bool>($"game/{GameData.GameId}/pass/{GameData.PlayerId}");
        }

        public async Task PlaceTile(BoardPlace bp)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/placetile/{GameData.PlayerId}", bp);
        }

        public async Task PlayBoardAction(BoardAction boardAction)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/boardaction/{GameData.PlayerId}", boardAction);
        }

        public async Task PlayCard(CardActionPlay cardAction)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/play/{GameData.PlayerId}", cardAction);
        }

        public async Task PlayCardAction(Action cardAction)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/cardaction/{GameData.PlayerId}", cardAction);
        }

        public async Task PlayConvertResourcesAction(ResourceHandler rh)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/convert/{GameData.PlayerId}", rh);
        }

        public async Task SelectEffectTarget(ResourceEffectPlayerChooser choice)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/selectactiontarget/{GameData.PlayerId}", choice);
        }

        public async Task<List<Board>> GetBoards()
        {
            return await TmDigitalClientRequestHandler.Instance.Request<List<Board>>("marsboard/boardslist");

        }

        public async Task<bool> EnsureInit()
        {
            return await TmDigitalClientRequestHandler.Instance.Request<bool>("game/ensureinit");
        }

        public async Task<Player> SendSetup(GameSetupSelection gSetup)
        {
            return await TmDigitalClientRequestHandler.Instance.Post<GameSetupSelection, Player>("game/addplayer/setupplayer", gSetup);
        }

        public async Task Skip()
        {
            await TmDigitalClientRequestHandler.Instance.Request<bool>($"game/{GameData.GameId}/skip/{GameData.PlayerId}");
        }

        public async Task<bool> StartGame(Guid selectedSessionGameSessionId)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<bool>($"game/start/{selectedSessionGameSessionId}");
        }

        public async Task VerifyCardPlayability(PatentSelector patent)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/verify/{GameData.PlayerId}", patent.Patent);
        }

        public async Task VerifyResourcesModifiedCostCardPlayability(PlayCardWithResources mod)
        {
            await TmDigitalClientRequestHandler.Instance.Post($"game/{GameData.GameId}/verifywithresources/{GameData.PlayerId}", mod);
        }
    }
}