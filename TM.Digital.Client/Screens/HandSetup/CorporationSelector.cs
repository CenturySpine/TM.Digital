using TM.Digital.Model.Corporations;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.HandSetup
{
    public class CorporationSelector : NotifierBase
    {
        private bool _isSelected;
        public Corporation Corporation { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
    }
}