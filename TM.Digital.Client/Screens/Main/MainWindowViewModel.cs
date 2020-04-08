using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.Screens.Menu;
using TM.Digital.Client.Screens.Wait;
using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;

namespace TM.Digital.Client.Screens.Main
{
    public class MainWindowViewModel : NotifierBase
    {
        public WaitingGameScreenViewModel WaitVm { get; }
        private Board _board;
        private PlayerSelector _currentPlayer;
        private Guid _gameId;
        private bool _isBoardLocked;

        private string _playerName;
        private string _server;
        private HubConnection connection;
        private GameSetupViewModel _gameSetupVm;

        public MainWindowViewModel(MainMenuViewModel menuVm, WaitingGameScreenViewModel waitVm)
        {
            MenuVm = menuVm;

            WaitVm = waitVm;

            MenuVm.IsVisible = true;
            MenuVm.GameStarted += MenuVm_GameCreated;
            MenuVm.GameJoined += MenuVm_GameJoined;
        }

        public Board Board
        {
            get => _board;
            set { _board = value; OnPropertyChanged(nameof(Board)); }
        }

        public PlayerSelector CurrentPlayer
        {
            get => _currentPlayer;
            set { _currentPlayer = value; OnPropertyChanged(nameof(CurrentPlayer)); }
        }

        public Guid GameId
        {
            get => _gameId;
            set { _gameId = value; OnPropertyChanged(nameof(GameId)); }
        }

        public bool IsBoardLocked
        {
            get => _isBoardLocked;
            set { _isBoardLocked = value; OnPropertyChanged(nameof(IsBoardLocked)); }
        }

        public MainMenuViewModel MenuVm { get; }

        public List<Player> OtherPlayers { get; set; }

        public string PlayerName
        {
            get => _playerName;
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }

        public RelayCommand Refresh { get; set; }

        public RelayCommand SelectBoardPlace { get; set; }

        public RelayCommand SelectCardCommand { get; set; }

        public string Server
        {
            get => _server;
            set { _server = value; OnPropertyChanged(nameof(Server)); }
        }

        public async Task Initialize()
        {
            await Task.CompletedTask;
            OtherPlayers = new List<Player>();
            Refresh = new RelayCommand(ExecuteRefresh);

            SelectCardCommand = new RelayCommand(ExecuteSelectCard, CanExecuteSelectCard);
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
                if (Guid.Parse(user) != WaitVm.PlayerId)
                {
                    Setup(message);
                }
            });
        }

        private void Setup(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<GameSetup>(message);
            GameSetupVm = new GameSetupViewModel();
            GameSetupVm.Setupcompleted += GameSetupVm_Setupcompleted;
            GameSetupVm.Setup(gameResult2, true);
        }

        private async void GameSetupVm_Setupcompleted(GameSetupViewModel vm)
        {
            GameSetupVm.Setupcompleted -= GameSetupVm_Setupcompleted;
            if (GameSetupVm.CorporationChoices.Any())
            {
                var player = await TmDigitalClientRequestHandler.Instance.Post<GameSetupSelection, Player>(
                    "game/addplayer/setupplayer", new GameSetupSelection
                    {
                        Corporation = GameSetupVm.CorporationChoices.First(c => c.IsSelected).Corporation,
                        BoughtCards = GameSetupVm.PatentChoices.Where(p => p.IsSelected).Select(p => p.Patent).ToList(),
                        PlayerId = GameSetupVm.PlayerId,
                        GameId = GameId,
                    });

                CurrentPlayer = new PlayerSelector(player);
            }
            WaitVm.Open("Waiting for other players to finish their setup");
        }

        public GameSetupViewModel GameSetupVm
        {
            get => _gameSetupVm;
            set { _gameSetupVm = value; OnPropertyChanged(nameof(GameSetupVm)); }
        }

        private bool CanExecuteSelectBoardPlace(object arg)
        {
            if (arg is BoardPlace bp)
            {
                return bp.CanBeChosed;
            }

            return false;
        }

        private bool CanExecuteSelectCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                return patent.Patent.CanBePlayed;
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

        private async void ExecuteSelectCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                if (!patent.Patent.CanBePlayed)
                    return;

                await TmDigitalClientRequestHandler.Instance.Post<Patent>($"game/{GameId}/play/{CurrentPlayer.Player.PlayerId}", patent.Patent);
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
        }

        private void UpdateGame(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<Game>(message);

            //update game with result
            Board = gameResult2.Board;
            CurrentPlayer = new PlayerSelector(gameResult2.AllPlayers.First(p => p.PlayerId == CurrentPlayer.Player.PlayerId));
            //TODO update other players
        }
    }
}