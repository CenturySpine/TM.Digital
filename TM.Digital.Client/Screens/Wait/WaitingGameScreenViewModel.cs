using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.Main;
using TM.Digital.Client.Services;
using TM.Digital.Model;
using TM.Digital.Model.Game;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Wait
{
    public class WaitingGameScreenViewModel : NotifierBase, IComponentConfigurable
    {
        private readonly IApiProxy _apiCaller;
        private readonly LoggerViewModel _logger;

        private ObservableCollection<string> _incomingMessages;
        private string _initialMessage;
        private bool _isOwner;
        private bool _isVisible;
        //private Guid _playerId;
        private GameSessionInformation _session;
        public WaitingGameScreenViewModel(IApiProxy apiCaller, LoggerViewModel logger)
        {
            _apiCaller = apiCaller;
            _logger = logger;
            IncomingMessages = new ObservableCollection<string>();
            StartGameCommand = new RelayCommand(ExecuteStartGame, CanExecuteStartGame);
            CheckPlayersReady = new RelayCommand(ExecuteCheckPlayerReady, CanExecutePlayersReadyCheck);

        }

        public RelayCommand CheckPlayersReady { get; set; }

        public ObservableCollection<string> IncomingMessages
        {
            get { return _incomingMessages; }
            set { _incomingMessages = value; OnPropertyChanged(nameof(IncomingMessages)); }
        }

        public string InitialMessage
        {
            get { return _initialMessage; }
            set
            {
                _initialMessage = value;
                OnPropertyChanged(nameof(InitialMessage));
            }
        }

        public bool IsOwner
        {
            get { return _isOwner; }
            set
            {
                _isOwner = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        //public Guid PlayerId
        //{
        //    get { return _playerId; }
        //    set
        //    {
        //        _playerId = value;
        //        OnPropertyChanged(nameof(PlayerId));
        //    }
        //}

        public GameSessionInformation Session
        {
            get { return _session; }
            set
            {
                _session = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand StartGameCommand { get; set; }

        public void Open(string awaitingPlayers)
        {
            IsVisible = true;
            InitialMessage = awaitingPlayers;
        }

        private bool CanExecutePlayersReadyCheck(object arg)
        {
            return IsOwner && IsVisible;
        }

        private bool CanExecuteStartGame(object arg)
        {
            return IsOwner;
        }

        private void ExecuteCheckPlayerReady(object obj)
        {
            
        }
        private async void ExecuteStartGame(object obj)
        {
            if (await _apiCaller.StartGame(Session.GameSessionId))
            {
                IsVisible = false;
                IncomingMessages.Clear();
                InitialMessage = string.Empty;
            }
            else
            {
                MessageBox.Show("Can't start game...");
            }
        }

        public void Close()
        {
            IsVisible = false;
        }

        public void RegisterSubscriptions(HubConnection hubConnection)
        {
            hubConnection.On<string, string>(ServerPushMethods.PlayerJoined, (user, message) =>
            {
                if (GameData.PlayerId != Guid.Empty && Guid.Parse(user) != GameData.PlayerId)
                {
                    //display message
                    IncomingMessages.Add($"Player '{message}'");
                }
            });
            hubConnection.On<string, string>(ServerPushMethods.Playing, async (user, message) =>
            {
                await Task.CompletedTask;
                var receivedUser = Guid.Parse(user);
                var currentUser = GameData.PlayerId;
                _logger.Log($"Player's turn :  {receivedUser}");

                if (currentUser != receivedUser)
                {

                    _logger.Log($"Board locked");

                    Close();
                    //IsBoardLocked = true;

                }
                else
                {
                    _logger.Log($"Board unlocked");
                    Close();
                    //IsBoardLocked = false;

                }
            });
        }
    }
}