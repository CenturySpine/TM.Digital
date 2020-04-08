using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model.Cards;

namespace TM.Digital.Client.Screens.HandSetup
{
    public class PatentSelector : NotifierBase
    {
        private bool _isSelected;
        public Patent Patent { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
    }
}