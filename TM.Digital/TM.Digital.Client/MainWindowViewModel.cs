using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;

namespace TM.Digital.Client
{
    public class MainWindowViewModel : NotifierBase
    {
        private readonly PopupService _popup;
        public WaitingGameScreenViewModel WaitVm { get; }
        private Board _board;
        private PlayerSelector _currentPlayer;
        private Guid _gameId;
        private bool _isBoardLocked;
        private bool _isGameOwner;
        private string _lockedMessage;
        private string _playerName;
        private string _server;
        private HubConnection connection;

        public MainWindowViewModel(PopupService popup, MainMenuViewModel menuVm, WaitingGameScreenViewModel waitVm)
        {
            MenuVm = menuVm;

            _popup = popup;
            WaitVm = waitVm;

            MenuVm.IsVisible = true;
            MenuVm.GameStarted += MenuVm_GameCreated;
            MenuVm.GameJoined += MenuVm_GameJoined;
        }

        public RelayCommand AddPlayerCommand { get; set; }

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

        //public bool IsGameOwner
        //{
        //    get => _isGameOwner;
        //    set { _isGameOwner = value; OnPropertyChanged(nameof(IsGameOwner)); }
        //}


        //public string LockedMessage
        //{
        //    get => _lockedMessage;
        //    private set { _lockedMessage = value; OnPropertyChanged(nameof(LockedMessage)); }
        //}

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
            AddPlayerCommand = new RelayCommand(ExecuteAddPlayer, CanExecuteAddPlayer);
            SelectCardCommand = new RelayCommand(ExecuteSelectCard, CanExecuteSelectCard);
            SelectBoardPlace = new RelayCommand(ExecuteSelectBoardPlace, CanExecuteSelectBoardPlace);
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
        }

        private bool CanExecuteAddPlayer(object arg)
        {
            return GameId != Guid.Empty && !string.IsNullOrEmpty(PlayerName);
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

        private void ExecuteAddPlayer(object obj)
        {
            CallErrorHandler.Handle(async () =>
            {
                var gameSetup =
                    await TmDigitalClientRequestHandler.Instance.Request<GameSetup>($"game/addplayer/{GameId}/" + PlayerName + "/" + true);

                var result = _popup.ShowGameSetup(gameSetup);

                //if (result.CorporationChoices.Any())
                {
                    var player = await TmDigitalClientRequestHandler.Instance.Post<GameSetupSelection, Player>("game/addplayer/setupplayer", new GameSetupSelection
                    {
                        Corporation = result.CorporationChoices.First(c => c.IsSelected).Corporation,
                        BoughtCards = result.PatentChoices.Where(p => p.IsSelected).Select(p => p.Patent).ToList(),
                        PlayerId = result.PlayerId,
                        GameId = GameId,
                    });

                    CurrentPlayer = new PlayerSelector(player);
                }
            });
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

            //LockedMessage = "Awaiting players...";
            //_popup.ShowLockedOverlay();
        }

        private async void MenuVm_GameJoined(Player joindPlayer)
        {
            await GetBoard();
            WaitVm.IsOwner = false;

            IsBoardLocked = true;

            WaitVm.PlayerId = joindPlayer.PlayerId;
            WaitVm.Open("....Waiting for game owner to start game...");


            //IsGameOwner = false;
            //await GetBoard();
            //IsBoardLocked = true;
            //LockedMessage = "....Waiting for game owner to start game...";
            //_popup.ShowLockedOverlay();
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

    internal static class CallErrorHandler
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