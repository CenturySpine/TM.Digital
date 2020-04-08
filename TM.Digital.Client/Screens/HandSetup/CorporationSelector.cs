using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model.Corporations;

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