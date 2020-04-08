using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TM.Digital.Model.Game;

namespace TM.Digital.Client
{
    public class WaitingGameScreenViewModel : NotifierBase
    {
        private readonly IApiProxy _apiCaller;
        
        private string _initialMessage;
        private bool _isOwner;
        private Guid _playerId;
        private ObservableCollection<string> _incommingMessages;
        private GameSessionInformation _session;
        private bool _isVisible;

        public WaitingGameScreenViewModel(IApiProxy apiCaller)
        {
            _apiCaller = apiCaller;
            IncommingMessages = new ObservableCollection<string>();
            StartGameCommand = new RelayCommand(ExecuteStartGame, CanExecuteStartGame);
        }

        private bool CanExecuteStartGame(object arg)
        {
            return IsOwner;
        }

        private void ExecuteStartGame(object obj)
        {
            IsVisible = false;
            IncommingMessages.Clear();
            InitialMessage = string.Empty;
        }

        public RelayCommand StartGameCommand { get; set; }

        public Guid PlayerId
        {
            get { return _playerId; }
            set
            {
                _playerId = value;
                OnPropertyChanged(nameof(PlayerId));
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

        public string InitialMessage
        {
            get { return _initialMessage; }
            set
            {
                _initialMessage = value;
                OnPropertyChanged(nameof(InitialMessage));
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public ObservableCollection<string> IncommingMessages
        {
            get { return _incommingMessages; }
            set { _incommingMessages = value; OnPropertyChanged(nameof(IncommingMessages)); }
        }

        public GameSessionInformation Session
        {
            get { return _session; }
            set { _session = value;
                OnPropertyChanged(nameof(Session));
            }
        }

        public void Open(string awaitingPlayers)
        {
            IsVisible = true;
            InitialMessage = awaitingPlayers;
        }
    }
}
