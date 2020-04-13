using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.HandSetup
{
    public abstract class GameBoardBaseViewModel : NotifierBase
    {
        public GameBoardBaseViewModel()
        {
            SelectCardCommand = new RelayCommand(ExecuteSelectCorporation, CanExecuteSelectCard);
        }

        protected abstract bool CanExecuteSelectCard(object arg);

        protected abstract void ExecuteSelectCorporation(object obj);

        public RelayCommand SelectCardCommand { get; set; }
    }
}