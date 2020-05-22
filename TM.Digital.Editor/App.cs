using System.Windows;
using TM.Digital.Editor.Board;

namespace TM.Digital.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            PackManager pm = new PackManager();
            await pm.Initialize();
            var mainVm = new MainViewModel(pm)
            {
                BoardViewModel = new BoardViewModel(pm)
            };

            //await mainVm.BoardViewModel.Initialize();

            var main = new MainWindow(mainVm);

            App.Current.MainWindow = main;
            Current.MainWindow?.Show();
        }
    }
}