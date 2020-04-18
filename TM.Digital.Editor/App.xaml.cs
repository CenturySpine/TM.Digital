using System.Windows;

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
            var mainVm = new MainViewModel();
            await mainVm.Initialize();
            
            var main = new MainWindow(mainVm);


            App.Current.MainWindow = main;
            Current.MainWindow?.Show();
        }
    }
}
