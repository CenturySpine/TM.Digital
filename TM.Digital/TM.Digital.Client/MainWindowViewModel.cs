using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        private bool _gameStarted;
        private string _playerName;
        private string _server;
        private Player _current;

        public MainWindowViewModel(PopupService popup)
        {
            _popup = popup;
        }

        public RelayCommand AddPlayerCommand { get; set; }

        public Board Board
        {
            get => _board;
            set { _board = value; OnPropertyChanged(nameof(Board)); }
        }

        public Player Current
        {
            get => _current;
            set { _current = value;OnPropertyChanged(nameof(Current)); }
        }

        public bool GameStarted
        {
            get => _gameStarted;
            set { _gameStarted = value; OnPropertyChanged(nameof(GameStarted)); }
        }

        public int NumberOfPlayers { get; set; } = 2;

        public List<Player> OtherPlayers { get; set; }

        public string PlayerName
        {
            get => _playerName;
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }

        public RelayCommand Refresh { get; set; }

        public string Server
        {
            get => _server;
            set { _server = value; OnPropertyChanged(nameof(Server)); }
        }

        public RelayCommand StartGameCommand { get; set; }

        public async Task Initialize()
        {
            await Task.CompletedTask;
            OtherPlayers = new List<Player>();
            Refresh = new RelayCommand(ExecuteRefresh);
            StartGameCommand = new RelayCommand(ExecuteStartGame, CanExecuteStartGame);
            AddPlayerCommand = new RelayCommand(ExecuteAddPlayer, CanExecuteAddPlayer);

        }
        private bool CanExecuteAddPlayer(object arg)
        {
            return GameStarted && !string.IsNullOrEmpty(PlayerName);
        }

        private bool CanExecuteStartGame(object arg)
        {
            return  /*&& !string.IsNullOrEmpty(Server)*/  NumberOfPlayers > 0 && NumberOfPlayers <= 5;
        }

        private void ExecuteAddPlayer(object obj)
        {
            CallErrorHandler.Handle(async () =>
            {
                var gameSetup =
                    await TmDigitalClientRequestHandler.Instance.Request<GameSetup>("game/addplayer/" + PlayerName);

                var result = _popup.ShowGameSetup(gameSetup);

                if (result.CorporationChoices.Any())
                {
                    var player = await TmDigitalClientRequestHandler.Instance.Post<GameSetupSelection, Player>("game/addplayer/setup", new GameSetupSelection
                    {
                        Corporation = result.CorporationChoices.First(c => c.IsSelected).Corporation,
                        BoughtCards= result.PatentChoices.Where(p=>p.IsSelected).Select(p=>p.Patent).ToList(),
                        PlayerId = result.PlayerId
                    });

                    Current = player;
                }
            });


        }
        private async void ExecuteRefresh(object obj)
        {
            await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
            await GetBoard();
        }

        private void ExecuteStartGame(object obj)
        {

            CallErrorHandler.Handle(async () =>
            {
                GameStarted = await TmDigitalClientRequestHandler.Instance.Request<bool>("game/start/" + NumberOfPlayers);
                if(GameStarted)
                {
                    await GetBoard();
                }
            });
        }
        private async Task GetBoard()
        {
            Board = await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
        }
    }

    static class CallErrorHandler
    {
        internal static void Handle(Action a)
        {
            try
            {
                a();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Console.WriteLine(e);

            }
        }
    }
}