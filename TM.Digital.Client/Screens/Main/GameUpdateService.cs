using System.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Model;
using TM.Digital.Model.Game;

namespace TM.Digital.Client.Screens.Main
{
    public class GameUpdateService : IComponentConfigurable
    {
        private readonly BoardViewModel _board;
        private readonly PlayerSelector _player;

        public GameUpdateService(BoardViewModel board, PlayerSelector player)
        {
            _board = board;
            _player = player;
        }
        public void RegisterSubscriptions(HubConnection hubConnection)
        {
            hubConnection.On<string, string>(ServerPushMethods.RecieveGameUpdate, (user, message) =>
            {
                if (user == "PlayResult")
                {
                    UpdateGame(message);
                }
            });
        }

        private void UpdateGame(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<Game>(message);
            //if (_player?.Player != null)
            //{
                //update game with result
                _board.Update(gameResult2.Board);

                _player.Update(gameResult2.AllPlayers.First(p => p.PlayerId == GameData.PlayerId));
                //TODO update other players
            //}
        }
    }
}