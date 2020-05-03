using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.Main;
using TM.Digital.Client.Screens.Wait;
using TM.Digital.Client.Services;
using TM.Digital.Model.Board;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Menu
{
    public sealed class MainMenuViewModel : NotifierBase, IComponentConfigurable

    {
        private readonly IApiProxy _apiCaller;
        private readonly WaitingGameScreenViewModel _waitViewModel;
        private readonly BoardViewModel _board;
        private bool _isGameCreationVisible;
        private bool _isSessionListVisible;
        private bool _isVisible;
        private int _numberOfPlayers = 1;
        private string _playerName;
        private GameSessionInformation _selectedSession;
        private List<Board> _boardsChoices;
        private Board _selectedBoard;

        public MainMenuViewModel(IApiProxy apiCaller, WaitingGameScreenViewModel waitViewModel, BoardViewModel board)
        {
            PlayerName = "Bruno" + DateTime.Now.Millisecond;
            _apiCaller = apiCaller;
            _waitViewModel = waitViewModel;
            _board = board;
            GameSessionInformation = new ObservableCollection<GameSessionInformation>();

            ShowCreateGameCommand = new RelayCommand(ExecuteShowCreateGame);
            CreateGameCommand = new RelayCommand(ExecuteCreateGame, CanExecuteStartGame);

            ListGameSessionsCommand = new RelayCommand(ExecuteListGames, CanExecuteListGames);
            JoinGameCommand = new RelayCommand(ExecuteJoinGame, CanExecuteJoinGame);
        }

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
        private async Task MenuVm_GameCreated(GameSessionInformation gameSessionInformation)
        {
            //IsGameOwner = true;//TODO enable game start
            await _board.GetBoard(gameSessionInformation.Board);

            _waitViewModel.Session = gameSessionInformation;
            _waitViewModel.IsOwner = true;

            //IsBoardLocked = true;
            GameData.PlayerId = gameSessionInformation.OwnerId;
            _waitViewModel.Open("Awaiting players...");
        }

        private async Task MenuVm_GameJoined(Player joindPlayer, string selectedSessionBoard)
        {
            await _board.GetBoard(selectedSessionBoard);
            _waitViewModel.IsOwner = false;

            //IsBoardLocked = true;

            GameData.PlayerId = joindPlayer.PlayerId;
            _waitViewModel.Open("....Waiting for game owner to start game...");
        }
        private void ExecuteCreateGame(object obj)
        {
            if (!CreateGameCommand.CanExecute(null))
                return;

            CallErrorHandler.Handle(async () =>
            {



                var gameId = await _apiCaller.CreateNewGame(PlayerName, NumberOfPlayers, SelectedBoard.Name);
                IsGameCreationVisible = false;
                IsSessionListVisible = false;
                IsVisible = false;
                await MenuVm_GameCreated(gameId);

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
                    await MenuVm_GameJoined(joinResult, SelectedSession.Board);
                }
                else
                {
                    MessageBox.Show("Can't join game");
                }
            });
        }

        private async void ExecuteListGames(object obj)
        {
            bool init = await _apiCaller.EnsureInit();
            if (init)
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
        }

        private async void ExecuteShowCreateGame(object obj)
        {
            bool init = await _apiCaller.EnsureInit();
            if (init)
            {
                
                BoardsChoices = new List<Board>(await _apiCaller.GetBoards());
                SelectedBoard = BoardsChoices.First();
                IsGameCreationVisible = true;
                IsSessionListVisible = false;
            }
        }

        public List<Board> BoardsChoices
        {
            get => _boardsChoices;
            set { _boardsChoices = value; OnPropertyChanged(); }
        }

        public Board SelectedBoard
        {
            get => _selectedBoard;
            set { _selectedBoard = value; OnPropertyChanged(); }
        }

        public void RegisterSubscriptions(HubConnection hubConnection)
        {

        }

        public void Show()
        {
            IsVisible = true;
        }
    }
}