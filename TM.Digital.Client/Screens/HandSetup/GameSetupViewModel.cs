using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Model.Game;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.HandSetup
{
    public class GameSetupViewModel : GameBoardBaseViewModel
    {
        private bool _isVisible;
        private bool _isInitialSetup;

        public GameSetupViewModel()
        {
            SelectCardCommand = new RelayCommand(ExecuteSelectCorporation, CanExecuteSelectCard);
            CloseCommand = new RelayCommand(ExecuteClose, CanExecuteClose);
            CorporationChoices = new ObservableCollection<CorporationSelector>();
            PatentChoices = new ObservableCollection<PatentSelector>();
        }

        public event SetupCompletedEventHandler Setupcompleted;

        protected override bool CanExecuteSelectCard(object obj)
        {
            return true;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }

        private bool CanExecuteClose(object arg)
        {
            return CorporationChoices.Count(c => c.IsSelected) == 1 || !IsInitialSetup;
        }

        private void ExecuteClose(object obj)
        {
            IsVisible = false;
            OnSetupCompleted(this);
        }

        public RelayCommand CloseCommand { get; set; }

        public ObservableCollection<CorporationSelector> CorporationChoices { get; set; }
        public ObservableCollection<PatentSelector> PatentChoices { get; set; }

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

        public void Setup(GameSetup gameSetup, bool isInitialSetup)
        {
            IsInitialSetup = isInitialSetup;
            PlayerId = gameSetup.PlayerId;
            GameId = gameSetup.GameId;
            foreach (var gameSetupCorporation in gameSetup.Corporations)
            {
                CorporationChoices.Add(new CorporationSelector { Corporation = gameSetupCorporation });
            }
            foreach (var gameSetupPatent in gameSetup.Patents)
            {
                PatentChoices.Add(new PatentSelector { Patent = gameSetupPatent, IsSetup = true});
            }

            IsVisible = true;
        }

        public Guid GameId { get; set; }

        public bool IsInitialSetup
        {
            get => _isInitialSetup;
            set { _isInitialSetup = value; OnPropertyChanged(nameof(IsInitialSetup)); }
        }

        public Guid PlayerId { get; set; }

        protected virtual void OnSetupCompleted(GameSetupViewModel vm)
        {
            Setupcompleted?.Invoke(vm);
        }
    }
}