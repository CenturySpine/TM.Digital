using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SimpleInjector;
using TM.Digital.Model.Game;
using Color = System.Drawing.Color;

namespace TM.Digital.Client
{
    public class PopupService
    {
        private readonly Container _container;

        public PopupService(Container container)
        {
            _container = container;
        }

        public GameSetupViewModel ShowGameSetup(GameSetup gameSetup)
        {
            var window = _container.GetInstance<GameSetupWindow>();
            if (window.DataContext is GameSetupViewModel vm)
            {
                vm.Setup(gameSetup);
                window.ShowDialog();

                return vm;
            }

            return null;
        }

        public void ShowLockedOverlay()
        {
            if (Application.Current.MainWindow != null)
            {
                var root = (Grid) Application.Current.MainWindow.Content;

                if (root.Children[^1] is FrameworkElement overlay) overlay.Visibility = Visibility.Visible;
            }
        }

        public void RemoveLockedMessage()
        {
            if (Application.Current.MainWindow != null)
            {
                var root = (Grid)Application.Current.MainWindow.Content;

                root.Children.RemoveAt(root.Children.Count-1);
            }
        }
    }
}