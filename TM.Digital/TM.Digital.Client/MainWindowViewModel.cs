using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;

namespace TM.Digital.Client
{
    public class MainWindowViewModel : NotifierBase
    {
        private readonly PopupService _popup;

        private Board _board;
        private string _playerName;
        private string _server;

        public Board Board
        {
            get => _board;
            set { _board = value; OnPropertyChanged(nameof(Board)); }
        }

        public MainWindowViewModel(PopupService popup)
        {
            _popup = popup;
        }
        public async Task Initialize()
        {
            await Task.CompletedTask;
            OtherPlayers = new List<Player>();
            Refresh = new RelayCommand(ExecuteRefresh);
            StartGameCommand = new RelayCommand(ExecuteStart, CanExecuteStart);
            AddPlayerCommand = new RelayCommand(ExecuteAddPlayer, CanExecuteAddPlayer);

        }

        public Player Current { get; set; }
        public List<Player> OtherPlayers { get; set; }

        private bool CanExecuteAddPlayer(object arg)
        {
            return true;
        }

        private async void ExecuteAddPlayer(object obj)
        {
            var gameSetup = await TmDigitalClientRequestHandler.Instance.Request<GameSetup>("game/addplayer");
            var result = _popup.ShowGameSetup(gameSetup);
            await TmDigitalClientRequestHandler.Instance.Post<GameSetupSelection>("game/addplayer/setup",new GameSetupSelection
            {
                Corporation = result.CorporationChoices.First(c=>c.IsSelected).Corporation,
                PlayerId = result.PlayerId
            });
        }

        public RelayCommand AddPlayerCommand { get; set; }

        private async void ExecuteStart(object obj)
        {

            await GetBoard();
        }

        private bool CanExecuteStart(object arg)
        {
            return !string.IsNullOrEmpty(PlayerName) && !string.IsNullOrEmpty(Server);
        }

        private async Task GetBoard()
        {
            Board = await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
        }

        private async void ExecuteRefresh(object obj)
        {
            await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
            await GetBoard();
        }

        public RelayCommand Refresh { get; set; }

        public string PlayerName
        {
            get => _playerName;
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }

        public string Server
        {
            get => _server;
            set { _server = value; OnPropertyChanged(nameof(Server)); }
        }

        public RelayCommand StartGameCommand { get; set; }
    }
}