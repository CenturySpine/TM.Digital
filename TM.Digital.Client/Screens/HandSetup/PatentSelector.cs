using System.Collections.Generic;
using System.Windows.Input;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.HandSetup
{
    public class PatentSelector : NotifierBase
    {
        public PatentSelector()
        {
            MineralsPatentModifiersSummary = new MineralsPatentModifiersSummary();
        }
        private bool _isSelected;
        public Patent Patent { get; set; }
        public MineralsPatentModifiersSummary MineralsPatentModifiersSummary { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
    }

    public class MineralsPatentModifiersSummary : NotifierBase
    {
        private int _modifiedRessourceCost;

        public MineralsPatentModifiersSummary()
        {
            MineralsPatentModifier = new List<MineralsPatentModifier>();
        }
        public List<MineralsPatentModifier> MineralsPatentModifier { get; set; }

        public int ModifiedRessourceCost
        {
            get => _modifiedRessourceCost;
            set { _modifiedRessourceCost = value; OnPropertyChanged(nameof(ModifiedRessourceCost)); }
        }
    }
    public class MineralsPatentModifier : NotifierBase
    {
        private int _unitsUsed;


        public MineralsPatentModifier()
        {

            AddUnitUsage = new RelayCommand(ExecuteAddUnit, CanExecuteAdd);
            RemoveUnitUsage = new RelayCommand(ExecuteRemove, CanExecuteRemove);
        }

        private bool CanExecuteRemove(object arg)
        {
            return UnitsUsed > 0;
        }

        private bool CanExecuteAdd(object arg)
        {
            return UnitsUsed < MaxUsage;
        }

        private void ExecuteRemove(object obj)
        {
            UnitsUsed--;
            CommandManager.InvalidateRequerySuggested();
        }

        private void ExecuteAddUnit(object obj)
        {
            UnitsUsed++;
            CommandManager.InvalidateRequerySuggested();
        }

        public ResourceType ResourceType { get; set; }

        public int UnitsUsed
        {
            get => _unitsUsed;
            set { _unitsUsed = value; OnPropertyChanged((nameof(UnitsUsed))); }
        }


        public RelayCommand AddUnitUsage { get; set; }
        public RelayCommand RemoveUnitUsage { get; set; }
        public int MaxUsage { get; set; }
    }
}