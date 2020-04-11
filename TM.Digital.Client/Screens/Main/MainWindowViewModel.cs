using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.Screens.Menu;
using TM.Digital.Client.Screens.Wait;
using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace TM.Digital.Client.Screens.Main
{
    public class MainWindowViewModel : NotifierBase
    {
        public WaitingGameScreenViewModel WaitVm { get; }
        
        private Board _board;
        private PlayerSelector _currentPlayer;

        private bool _isBoardLocked;

        private string _playerName;
        private string _server;
        private HubConnection connection;
        private GameSetupViewModel _gameSetupVm;
        private ObservableCollection<string> _logs;
        private RelayCommand _selectBoardPlace;
        private ActionChoiceViewModel _actionChoiceViewModel;

        public MainWindowViewModel(MainMenuViewModel menuVm, WaitingGameScreenViewModel waitVm, ActionChoiceViewModel actionChoice)
        {
            MenuVm = menuVm;

            WaitVm = waitVm;
            ActionChoiceViewModel = actionChoice;

            MenuVm.IsVisible = true;
            MenuVm.GameStarted += MenuVm_GameCreated;
            MenuVm.GameJoined += MenuVm_GameJoined;
            ActionChoiceViewModel.ChoiceSelected += ActionChoiceViewModel_ChoiceSelected;
        }



        public Board Board
        {
            get { return _board; }
            set { _board = value; OnPropertyChanged(nameof(Board)); }
        }

        public PlayerSelector CurrentPlayer
        {
            get { return _currentPlayer; }
            set { _currentPlayer = value; OnPropertyChanged(nameof(CurrentPlayer)); }
        }

        public bool IsBoardLocked
        {
            get { return _isBoardLocked; }
            set { _isBoardLocked = value; OnPropertyChanged(nameof(IsBoardLocked)); }
        }

        public MainMenuViewModel MenuVm { get; }

        public List<Player> OtherPlayers { get; set; }

        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }

        public RelayCommand Refresh { get; set; }

        public RelayCommand SelectBoardPlace
        {
            get => _selectBoardPlace;
            set { _selectBoardPlace = value; OnPropertyChanged(nameof(SelectBoardPlace)); }
        }

        public RelayCommand SelectCardCommand { get; set; }

        public string Server
        {
            get { return _server; }
            set { _server = value; OnPropertyChanged(nameof(Server)); }
        }

        public async Task Initialize()
        {
            await Task.CompletedTask;
            Logs = new ObservableCollection<string>();
            OtherPlayers = new List<Player>();
            Refresh = new RelayCommand(ExecuteRefresh);

            //SelectCardCommand = new RelayCommand(ExecuteSelectCard, CanExecuteSelectCard);
            SelectBoardPlace = new RelayCommand(ExecuteSelectBoardPlace, CanExecuteSelectBoardPlace);
            await ConnectToHub();
        }

        private async Task ConnectToHub()
        {
            connection = new HubConnectionBuilder()
                .WithUrl(@"http://localhost:50154/ClientNotificationHub")
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            connection.On<string, string>(ServerPushMethods.RecieveGameUpdate, (user, message) =>
            {
                if (user == "PlayResult")
                {
                    UpdateGame(message);
                }
            });
            connection.On<string, string>(ServerPushMethods.PlaceTileRequest, (user, message) =>
            {
                if (Guid.Parse(user) == CurrentPlayer.Player.PlayerId)
                {
                    ShowTilesPlacement(message);
                }
            });
            connection.On<string, string>(ServerPushMethods.PlayerJoined, (user, message) =>
            {
                if (Guid.Parse(user) != WaitVm.PlayerId)
                {
                    //display message
                    WaitVm.IncommingMessages.Add($"Player '{message}' joined the game");
                }
            });

            connection.On<string, string>(ServerPushMethods.SetupChoice, (user, message) =>
            {
                if (Guid.Parse(user) == WaitVm.PlayerId)
                {
                    Setup(message);
                }
            });
            connection.On<string, string>(ServerPushMethods.ResourceEffectForOtherPlayer, (user, message) =>
            {
                if (Guid.Parse(user) == WaitVm.PlayerId)
                {
                    DisplayResourceEffectChoice(message);
                }
            });

            connection.On<string, string>(ServerPushMethods.LogReceived, (user, message) => { AddLog(message); });
            connection.On<string, string>(ServerPushMethods.Playing, async (user, message) =>
            {

                var receivedUser = Guid.Parse(user);
                var currentUser = WaitVm.PlayerId;
                AddLog($"Received notification :user's turn : {receivedUser} {message}, current user i {currentUser}");

                if (currentUser != receivedUser)
                {

                    AddLog($"Board locked");

                    WaitVm.IsVisible = false;
                    IsBoardLocked = true;

                }
                else
                {
                    AddLog($"Board unlocked");
                    WaitVm.IsVisible = false;
                    IsBoardLocked = false;

                }
            });
        }

        private void DisplayResourceEffectChoice(string message)
        {
            ResourceEffectPlayerChooserList chooser = JsonConvert.DeserializeObject<ResourceEffectPlayerChooserList>(message);

            ActionChoiceViewModel.Setup(chooser);
            ActionChoiceViewModel.IsVisible = true;
        }
        private async void ActionChoiceViewModel_ChoiceSelected(ResourceEffectPlayerChooser choice)
        {
            
            ActionChoiceViewModel.SelectedChoice = null;
            ActionChoiceViewModel.IsVisible = false;
            

            await TmDigitalClientRequestHandler.Instance.Post<ResourceEffectPlayerChooser>($"game/{GameId}/selectactiontarget/{CurrentPlayer.Player.PlayerId}", choice);

        }

        public ActionChoiceViewModel ActionChoiceViewModel
        {
            get => _actionChoiceViewModel;
            set { _actionChoiceViewModel = value; OnPropertyChanged(nameof(ActionChoiceViewModel)); }
        }

        private void AddLog(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { Logs.Insert(0, message); }));
        }

        private void Setup(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<GameSetup>(message);
            WaitVm.IsVisible = false;
            GameSetupVm = new GameSetupViewModel();
            GameSetupVm.Setupcompleted += GameSetupVm_SetupCompleted;
            GameSetupVm.Setup(gameResult2, gameResult2.IsInitialSetup);
        }

        private async void GameSetupVm_SetupCompleted(GameSetupViewModel vm)
        {
            WaitVm.Open("Waiting for other players to finish their setup");
            GameSetupVm.Setupcompleted -= GameSetupVm_SetupCompleted;
            if (GameSetupVm.CorporationChoices.Any() || !vm.IsInitialSetup)
            {

                var gSetup = new GameSetupSelection
                {
                    Corporation = GameSetupVm.CorporationChoices.ToDictionary(k => k.Corporation.Guid.ToString(), v => v.IsSelected),
                    BoughtCards = GameSetupVm.PatentChoices.ToDictionary(k => k.Patent.Guid.ToString(), v => v.IsSelected),
                    PlayerId = GameSetupVm.PlayerId,
                    GameId = vm.GameId,
                };
                GameId = vm.GameId;
                var gameResult2 = await TmDigitalClientRequestHandler.Instance.Post<GameSetupSelection, Player>("game/addplayer/setupplayer", gSetup);
                UpdatePlayerSelector(gameResult2);
            }

        }

        private void UpdatePlayerSelector(Player player)
        {
            if (CurrentPlayer != null)
            {
                CurrentPlayer.PlayerSkipped -= CurrentPlayer_PlayerSkipped;
                CurrentPlayer.PlayerPassed -= CurrentPlayerPlayerPassed;
            }
            CurrentPlayer = new PlayerSelector(player, GameId);
            CurrentPlayer.PlayerSkipped += CurrentPlayer_PlayerSkipped;
            CurrentPlayer.PlayerPassed += CurrentPlayerPlayerPassed;
        }

        public Guid GameId { get; set; }

        private async void CurrentPlayerPlayerPassed(Guid playerid)
        {
            await TmDigitalClientRequestHandler.Instance.Request<bool>($"game/{GameId}/pass/{playerid}");
        }

        private async void CurrentPlayer_PlayerSkipped(Guid playerid)
        {
            await TmDigitalClientRequestHandler.Instance.Request<bool>($"game/{GameId}/skip/{playerid}");
        }

        public GameSetupViewModel GameSetupVm
        {
            get { return _gameSetupVm; }
            set { _gameSetupVm = value; OnPropertyChanged(nameof(GameSetupVm)); }
        }

        public ObservableCollection<string> Logs
        {
            get { return _logs; }
            set { _logs = value; OnPropertyChanged(nameof(Logs)); }
        }

        private bool CanExecuteSelectBoardPlace(object arg)
        {
            if (arg is BoardPlace bp)
            {
                return bp.CanBeChosed;
            }

            return false;
        }



        private async void ExecuteRefresh(object obj)
        {
            await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
            await GetBoard();
        }

        private async void ExecuteSelectBoardPlace(object obj)
        {
            if (obj is BoardPlace bp)
            {
                await TmDigitalClientRequestHandler.Instance.Post<BoardPlace>($"game/{GameId}/placetile/{CurrentPlayer.Player.PlayerId}", bp);
            }
        }



        private async Task GetBoard()
        {
            Board = await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
        }

        private async void MenuVm_GameCreated(GameSessionInformation gameSessionInformation)
        {
            //IsGameOwner = true;//TODO enable game start
            await GetBoard();

            WaitVm.Session = gameSessionInformation;
            WaitVm.IsOwner = true;

            IsBoardLocked = true;
            WaitVm.PlayerId = gameSessionInformation.OwnerId;
            WaitVm.Open("Awaiting players...");
        }

        private async void MenuVm_GameJoined(Player joindPlayer)
        {
            await GetBoard();
            WaitVm.IsOwner = false;

            IsBoardLocked = true;

            WaitVm.PlayerId = joindPlayer.PlayerId;
            WaitVm.Open("....Waiting for game owner to start game...");
        }

        private void ShowTilesPlacement(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<Board>(message);
            Board = gameResult2;
            CommandManager.InvalidateRequerySuggested();
        }

        private void UpdateGame(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<Game>(message);
            if (CurrentPlayer?.Player != null)
            {
                //update game with result
                Board = gameResult2.Board;

                UpdatePlayerSelector(gameResult2.AllPlayers.First(p => p.PlayerId == CurrentPlayer.Player.PlayerId));
                //TODO update other players
            }
        }
    }
}