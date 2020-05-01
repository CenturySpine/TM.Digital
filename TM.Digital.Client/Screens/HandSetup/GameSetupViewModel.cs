using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.Main;
using TM.Digital.Client.Screens.Wait;
using TM.Digital.Client.Services;
using TM.Digital.Model;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.HandSetup
{
    public class GameSetupViewModel : GameBoardBaseViewModel, IComponentConfigurable
    {
        private readonly IApiProxy _apiProxy;
        private readonly PlayerSelector _playerSelector;
        private readonly WaitingGameScreenViewModel _waitVm;
        private bool _isInitialSetup;
        private bool _isVisible;

        public GameSetupViewModel(WaitingGameScreenViewModel waitVm, IApiProxy apiProxy, PlayerSelector playerSelector)
        {
            _waitVm = waitVm;
            _apiProxy = apiProxy;
            _playerSelector = playerSelector;
            SelectCardCommand = new RelayCommand(ExecuteSelectCorporation, CanExecuteSelectCard);
            CloseCommand = new RelayCommand(ExecuteClose, CanExecuteClose);
            CorporationChoices = new ObservableCollection<CorporationSelector>();
            PatentChoices = new ObservableCollection<PatentSelector>();
        }

        public RelayCommand CloseCommand { get; set; }

        public ObservableCollection<CorporationSelector> CorporationChoices { get; set; }

        public bool IsInitialSetup
        {
            get => _isInitialSetup;
            set { _isInitialSetup = value; OnPropertyChanged(nameof(IsInitialSetup)); }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }

        public ObservableCollection<PatentSelector> PatentChoices { get; set; }

        public void RegisterSubscriptions(HubConnection hubConnection)
        {
            hubConnection.On<string, string>(ServerPushMethods.SetupChoice, (user, message) =>
            {
                if (Guid.Parse(user) == GameData.PlayerId)
                {
                    Setup(message);
                }
            });
        }

        protected override bool CanExecuteSelectCard(object obj)
        {
            return true;
        }

        protected override void ExecuteSelectCorporation(object obj)
        {
            if (obj is CorporationSelector select)
            {
                select.IsSelected = !select.IsSelected;
                if (select.IsSelected)
                    foreach (var corporationSelector in CorporationChoices.Except(new List<CorporationSelector> { select }))
                    {
                        corporationSelector.IsSelected = false;
                    }
            }

            if (obj is PatentSelector patent)
            {
                patent.IsSelected = !patent.IsSelected;
            }
        }

        private bool CanExecuteClose(object arg)
        {
            return CorporationChoices.Count(c => c.IsSelected) == 1 || !IsInitialSetup;
        }

        private void ExecuteClose(object obj)
        {
            IsVisible = false;
            GameSetupVm_SetupCompleted();
        }

        private async void GameSetupVm_SetupCompleted()
        {
            _waitVm.Open("Waiting for other players to finish their setup");

            if (CorporationChoices.Any() || !IsInitialSetup)
            {
                var gSetup = new GameSetupSelection
                {
                    Corporation = CorporationChoices.ToDictionary(k => k.Corporation.Guid.ToString(), v => v.IsSelected),
                    BoughtCards = PatentChoices.ToDictionary(k => k.Patent.Guid.ToString(), v => v.IsSelected),
                    PlayerId = GameData.PlayerId,
                    GameId = GameData.GameId,
                };

                var gameResult2 =
                   await _apiProxy.SendSetup(gSetup);
                    
                _playerSelector.Update(gameResult2);
            }
        }

        private void Setup(GameSetup gameSetup, bool isInitialSetup)
        {
            IsInitialSetup = isInitialSetup;
            GameData.PlayerId = gameSetup.PlayerId;
            GameData.GameId = gameSetup.GameId;
            CorporationChoices.Clear();
            PatentChoices.Clear();
            foreach (var gameSetupCorporation in gameSetup.Corporations)
            {
                CorporationChoices.Add(new CorporationSelector { Corporation = gameSetupCorporation });
            }
            foreach (var gameSetupPatent in gameSetup.Patents)
            {
                PatentChoices.Add(new PatentSelector { Patent = gameSetupPatent, IsSetup = true });
            }

            IsVisible = true;
        }

        private void Setup(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<GameSetup>(message);
            _waitVm.Close();

            Setup(gameResult2, gameResult2.IsInitialSetup);
        }
    }
}