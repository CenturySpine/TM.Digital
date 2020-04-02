using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Model.Game;

namespace TM.Digital.Client
{
    public class GameSetupViewModel : ClosableViewModel
    {
        public GameSetupViewModel()
        {
            SelectCorporation = new RelayCommand(ExecuteSelectCorporation);
            CloseCommand = new RelayCommand(ExecuteClose, CanExecuteClose);
            ;
            CorporationChoices = new ObservableCollection<CorporationSelector>();
        }

        private bool CanExecuteClose(object arg)
        {
            return CorporationChoices.Count(c => c.IsSelected) == 1;
        }

        private void ExecuteClose(object obj)
        {
            OnClosed();
        }

        public RelayCommand CloseCommand { get; set; }

        public ObservableCollection<CorporationSelector> CorporationChoices { get; set; }

        public RelayCommand SelectCorporation { get; set; }

        private void ExecuteSelectCorporation(object obj)
        {
            if (obj is CorporationSelector select)
            {
                select.IsSelected = !select.IsSelected;
                if (select.IsSelected)
                    foreach (var corporationSelector in CorporationChoices.Except(new List<CorporationSelector>() { select }))
                    {
                        corporationSelector.IsSelected = false;
                    }
            }
        }

        public void Setup(GameSetup gameSetup)
        {
            PlayerId = gameSetup.PlayerId;
            foreach (var gameSetupCorporation in gameSetup.Corporations)
            {
                CorporationChoices.Add(new CorporationSelector() { Corporation = gameSetupCorporation });
            }
        }

        public Guid PlayerId { get; set; }
    }
}