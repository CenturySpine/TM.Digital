using System;
using System.Windows;
using TM.Digital.Model.Corporations;

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