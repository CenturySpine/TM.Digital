using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Game;

namespace TM.Digital.Client
{
    public class ClosableBehavior
    {
        private Window _owner;

        public ClosableBehavior(Window window)
        {
            _owner = window;
            if (_owner.DataContext is ClosableViewModel closable)
            {
                closable.Closed += Closable_Closed;
            }
        }

        private void Closable_Closed(object sender, EventArgs e)
        {
            if (_owner.DataContext is ClosableViewModel closable)
            {
                closable.Closed -= Closable_Closed;
            }
            _owner.Close();
            _owner = null;
        }
    }

    /// <summary>
    /// Interaction logic for GameSetupWindow.xaml
    /// </summary>
    public partial class GameSetupWindow : Window
    {
        private ClosableBehavior _close;

        public GameSetupWindow(GameSetupViewModel vm)
        {
            DataContext = vm;

            InitializeComponent();

            _close = new ClosableBehavior(this);
        }
    }

    public class ClosableViewModel : NotifierBase
    {
        public event EventHandler Closed;

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

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