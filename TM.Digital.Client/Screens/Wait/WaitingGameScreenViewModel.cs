using System;
using System.Collections.ObjectModel;
using System.Windows;
using TM.Digital.Client.Services;
using TM.Digital.Model.Game;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Wait
{
    public class WaitingGameScreenViewModel : NotifierBase
    {
        private readonly IApiProxy _apiCaller;

        private ObservableCollection<string> _incommingMessages;
        private string _initialMessage;
        private bool _isOwner;
        private bool _isVisible;
        private Guid _playerId;
        private GameSessionInformation _session;
        public WaitingGameScreenViewModel(IApiProxy apiCaller)
        {
            _apiCaller = apiCaller;
            IncommingMessages = new ObservableCollection<string>();
            StartGameCommand = new RelayCommand(ExecuteStartGame, CanExecuteStartGame);
            CheckPlayersReady = new RelayCommand(ExecuteCheckPlayerReady, CanExecutePlayersReadyCheck);

        }

        public RelayCommand CheckPlayersReady { get; set; }

        public ObservableCollection<string> IncommingMessages
        {
            get { return _incommingMessages; }
            set { _incommingMessages = value; OnPropertyChanged(nameof(IncommingMessages)); }
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
                OnPropertyChanged(nameof(IsOwner));
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public Guid PlayerId
        {
            get { return _playerId; }
            set
            {
                _playerId = value;
                OnPropertyChanged(nameof(PlayerId));
            }
        }

        public GameSessionInformation Session
        {
            get { return _session; }
            set
            {
                _session = value;
                OnPropertyChanged(nameof(Session));
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
                IncommingMessages.Clear();
                InitialMessage = string.Empty;
            }
            else
            {
                MessageBox.Show("Can't start game...");
            }
        }
    }
}