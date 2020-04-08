using System.Windows;

namespace TM.Digital.Client.Screens.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}