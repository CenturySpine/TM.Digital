using System.Collections.ObjectModel;
using System.Windows;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;

namespace TM.Digital.Client
{
    public delegate void GameCreatedEventHandler(GameSessionInformation gameId);

    public delegate void GameJoinSuccessEventHandler(Player joindPlayer);

    public sealed class MainMenuViewModel : NotifierBase
    {
        private readonly IApiProxy _apiCaller;
        private bool _isGameCreationVisible;
        private bool _isSessionListVisible;
        private bool _isVisible;
        private int _numberOfPlayers = 2;
        private string _playerName;
        private GameSessionInformation _selectedSession;

        public MainMenuViewModel(IApiProxy apiCaller)
        {
            _apiCaller = apiCaller;
            GameSessionInformation = new ObservableCollection<GameSessionInformation>();

            ShowCreateGameCommand = new RelayCommand(ExecuteShowCreateGame);
            CreateGameCommand = new RelayCommand(ExecuteCreatetGame, CanExecuteStartGame);

            ListGameSessionsCommand = new RelayCommand(ExecuteListGames, CanExecuteListGames);
            JoinGameCommand = new RelayCommand(ExecuteJoinGame, CanExecuteJoinGame);
        }

        public event GameJoinSuccessEventHandler GameJoined;

        public event GameCreatedEventHandler GameStarted;

        public RelayCommand CreateGameCommand { get; set; }
        public ObservableCollection<GameSessionInformation> GameSessionInformation { get; set; }

        public bool IsGameCreationVisible
        {
            get => _isGameCreationVisible;
            set { _isGameCreationVisible = value; OnPropertyChanged(nameof(IsGameCreationVisible)); }
        }

        public bool IsSessionListVisible
        {
            get => _isSessionListVisible;
            set { _isSessionListVisible = value; OnPropertyChanged(nameof(IsSessionListVisible)); }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }

        public RelayCommand JoinGameCommand { get; set; }

        public RelayCommand ListGameSessionsCommand { get; set; }

        public int NumberOfPlayers
        {
            get => _numberOfPlayers;
            set { _numberOfPlayers = value; OnPropertyChanged(nameof(NumberOfPlayers)); }
        }

        public string PlayerName
        {
            get => _playerName;
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }

        public GameSessionInformation SelectedSession
        {
            get => _selectedSession;
            set { _selectedSession = value; OnPropertyChanged(nameof(SelectedSession)); }
        }

        public RelayCommand ShowCreateGameCommand { get; set; }

        private void OnGameCreated(GameSessionInformation gameId)
        {
            GameStarted?.Invoke(gameId);
        }

        private void OnGameJoined(Player joindplayer)
        {
            GameJoined?.Invoke(joindplayer);
        }

        private bool CanExecuteJoinGame(object arg)
        {
            return SelectedSession != null;
        }

        private bool CanExecuteListGames(object arg)
        {
            return true;
        }

        private bool CanExecuteStartGame(object arg)
        {
            return !string.IsNullOrEmpty(PlayerName) && NumberOfPlayers > 0 && NumberOfPlayers <= 5;
        }

        private void ExecuteCreatetGame(object obj)
        {
            if (!CreateGameCommand.CanExecute(null))
                return;

            CallErrorHandler.Handle(async () =>
            {
                var gameId = await _apiCaller.CreateNewGame(PlayerName, NumberOfPlayers);
                IsGameCreationVisible = false;
                IsSessionListVisible = false;
                IsVisible = false;
                OnGameCreated(gameId);
            });
        }

        private void ExecuteJoinGame(object obj)
        {
            //TODO
            if (SelectedSession == null)
            {
                return;
            }

            if (SelectedSession.NumnerOfPlayers == 5)
            {
                MessageBox.Show("The selected game is full");
                return;
            }

            if (string.IsNullOrEmpty(PlayerName))
            {
                MessageBox.Show("You must enter your name");
                return;
            }

            CallErrorHandler.Handle(async () =>
            {
                var joinResult = await _apiCaller.JoinGame(SelectedSession.GameSessionId, PlayerName);
                if (joinResult != null)
                {
                    IsGameCreationVisible = false;
                    IsSessionListVisible = false;
                    IsVisible = false;
                    OnGameJoined(joinResult);
                }
                else
                {
                    MessageBox.Show("Can't join game");
                }
            });
        }

        private void ExecuteListGames(object obj)
        {
            IsGameCreationVisible = false;
            IsSessionListVisible = true;

            CallErrorHandler.Handle(async () =>
            {
                var sessions = await _apiCaller.GetGameSessions();

                GameSessionInformation.Clear();
                foreach (var gameSession in sessions.GameSessionsList)
                {
                    GameSessionInformation.Add(gameSession);
                }

                IsSessionListVisible = true;
            });
        }

        private void ExecuteShowCreateGame(object obj)
        {
            IsGameCreationVisible = true;
            IsSessionListVisible = false;
        }
    }
}