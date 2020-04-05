using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TM.Digital.Client
{
    public class PlayerSelector
    {
        public PlayerSelector(Player player)
        {
            Player = player;
            PatentsSelectors = new ObservableCollection<PatentSelector>(player.HandCards.Select(c => new PatentSelector { Patent = c }));
        }

        public Player Player { get; set; }
        public ObservableCollection<PatentSelector> PatentsSelectors { get; set; }
    }

    public class MainWindowViewModel : NotifierBase
    {
        private readonly PopupService _popup;

        private Board _board;
        private Guid _gameId;
        private string _playerName;
        private string _server;
        private PlayerSelector _currentPlayer;
        private HubConnection connection;

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
            SelectCardCommand = new RelayCommand(ExecuteSelectCard, CanExecuteSelectCard);
            connection = new HubConnectionBuilder()
                .WithUrl(@"http://localhost:50154/ClientNotificationHub")
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            connection.On<string, string>("ReceiveGameUpdate", (user, message) =>
            {
                if (user == "PlayResult")
                {
                    UpdateGame(message);
                }
            });
        }

        private void UpdateGame(string message)
        {
            //var gameResult = JsonSerializer.Deserialize<Game>(message, new JsonSerializerOptions
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                
            //});

            var gameResult2 = JsonConvert.DeserializeObject<Game>(message);

            //update game with result
            Board = gameResult2.Board;
            CurrentPlayer = new PlayerSelector(gameResult2.AllPlayers.First(p => p.PlayerId == CurrentPlayer.Player.PlayerId));
            //TODO update other players
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

        private bool CanExecuteSelectCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                return patent.Patent.CanBePlayed;
            }
            return false;
        }

        private bool CanExecuteAddPlayer(object arg)
        {
            return GameId != Guid.Empty && !string.IsNullOrEmpty(PlayerName);
        }

        private bool CanExecuteStartGame(object arg)
        {
            return  /*&& !string.IsNullOrEmpty(Server)*/  NumberOfPlayers > 0 && NumberOfPlayers <= 5;
        }

        public RelayCommand SelectCardCommand { get; set; }

        private void ExecuteAddPlayer(object obj)
        {
            CallErrorHandler.Handle(async () =>
            {
                var gameSetup =
                    await TmDigitalClientRequestHandler.Instance.Request<GameSetup>($"game/addplayer/{GameId}/" + PlayerName);

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

        private void ExecuteStartGame(object obj)
        {
            CallErrorHandler.Handle(async () =>
            {
                GameId = await TmDigitalClientRequestHandler.Instance.Request<Guid>("game/start/" + NumberOfPlayers);
                if (GameId != Guid.Empty)
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