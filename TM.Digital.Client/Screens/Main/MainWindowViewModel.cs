using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.Screens.Menu;
using TM.Digital.Client.Screens.Wait;
using TM.Digital.Model.Board;
using TM.Digital.Model.Player;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Main
{
    public class MainWindowViewModel : NotifierBase
    {
        private ActionChoiceViewModel _actionChoiceViewModel;

        //private Board _board;
        private PlayerSelector _currentPlayer;

        private GameSetupViewModel _gameSetupVm;
        private bool _isBoardLocked;
        private ObservableCollection<string> _logs;
        private string _playerName;
        private RelayCommand _selectBoardPlace;
        private string _server;
        private HubConnection hubConnection;
        private WaitingGameScreenViewModel _waitVm;
        private PlayerSelector _playerSelector;
        private MainMenuViewModel _menuVm;
        private LoggerViewModel _logger;
        private BoardViewModel _boardViewModel;

        public MainWindowViewModel(
            MainMenuViewModel menuVm,
            WaitingGameScreenViewModel waitVm,
            ActionChoiceViewModel actionChoice,
            GameSetupViewModel gameSetupViewModel,
            PlayerSelector playerSelector,
            LoggerViewModel logger,
            GameUpdateService updateService,
            BoardViewModel boardViewModel)
        {
            MenuVm = menuVm;
            WaitVm = waitVm;
            ActionChoiceViewModel = actionChoice;
            GameSetupVm = gameSetupViewModel;
            PlayerSelector = playerSelector;
            Logger = logger;
            UpdateService = updateService;
            BoardViewModel = boardViewModel;
        }

        public ActionChoiceViewModel ActionChoiceViewModel
        {
            get => _actionChoiceViewModel;
            set { _actionChoiceViewModel = value; OnPropertyChanged(); }
        }

        public BoardViewModel BoardViewModel
        {
            get => _boardViewModel;
            set { _boardViewModel = value; OnPropertyChanged(); }
        }

        public PlayerSelector CurrentPlayer
        {
            get { return _currentPlayer; }
            set { _currentPlayer = value; OnPropertyChanged(); }
        }

        public RelayCommand ExecuteActionCommand { get; set; }

        public GameSetupViewModel GameSetupVm
        {
            get { return _gameSetupVm; }
            set { _gameSetupVm = value; OnPropertyChanged(); }
        }

        public bool IsBoardLocked
        {
            get { return _isBoardLocked; }
            set { _isBoardLocked = value; OnPropertyChanged(); }
        }

        public LoggerViewModel Logger
        {
            get => _logger;
            set { _logger = value; OnPropertyChanged(); }
        }

        public MainMenuViewModel MenuVm
        {
            get => _menuVm;
            set { _menuVm = value; OnPropertyChanged(); }
        }

        public List<Player> OtherPlayers { get; set; }

        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; OnPropertyChanged(); }
        }

        public PlayerSelector PlayerSelector
        {
            get => _playerSelector;
            set { _playerSelector = value; OnPropertyChanged(); }
        }

        public RelayCommand Refresh { get; set; }
        public RelayCommand SelectCardCommand { get; set; }

        public string Server
        {
            get { return _server; }
            set { _server = value; OnPropertyChanged(nameof(Server)); }
        }

        public GameUpdateService UpdateService { get; }

        public WaitingGameScreenViewModel WaitVm
        {
            get => _waitVm;
            set { _waitVm = value; OnPropertyChanged(); }
        }

        public async Task Initialize()
        {
            await Task.CompletedTask;

            OtherPlayers = new List<Player>();
            Refresh = new RelayCommand(ExecuteRefresh);

            //SelectCardCommand = new RelayCommand(ExecuteSelectCard, CanExecuteSelectCard);

            await ConnectToHub();

            MenuVm.Show();
        }

        private async Task ConnectToHub()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(@"http://localhost:50154/ClientNotificationHub")
                .WithAutomaticReconnect()
                .Build();

            await hubConnection.StartAsync();

            ActionChoiceViewModel.RegisterSubscriptions(hubConnection);

            GameSetupVm.RegisterSubscriptions(hubConnection);

            WaitVm.RegisterSubscriptions(hubConnection);

            UpdateService.RegisterSubscriptions(hubConnection);

            BoardViewModel.RegisterSubscriptions(hubConnection);
        }

        private void ExecuteBoardAction(object obj)
        {
            if (obj is BoardAction ba)
            {
            }
        }

        private async void ExecuteRefresh(object obj)
        {
            //await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
            //await GetBoard();
        }

        //private void UpdatePlayerSelector(Player player)
        //{
        //    CurrentPlayer.Update(player);
        //}
    }
}