using System.Windows;
using System.Windows.Controls;

namespace TM.Digital.Editor.Board
{
    /// <summary>
    /// Interaction logic for BoardTester.xaml
    /// </summary>
    public partial class BoardTester : UserControl
    {
        public BoardTester()
        {
            
            InitializeComponent();
        }

        private void EditorGrid_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is BoardViewModel btv)
            {
                btv.Refresh();
            }
        }
    }
}
