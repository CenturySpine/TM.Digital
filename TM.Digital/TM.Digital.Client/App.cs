using SimpleInjector;
using System.Windows;

namespace TM.Digital.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SimpleInjector.Container ctn = new Container();

            //register
            ctn.Register<MainWindowViewModel>(Lifestyle.Singleton);
            ctn.Register<MainWindow>(Lifestyle.Transient);

            ctn.Register<GameSetupWindow>(Lifestyle.Transient);
            ctn.Register<GameSetupViewModel>(Lifestyle.Transient);

            ctn.Register<PopupService>(Lifestyle.Singleton);

            //resolve
            await ctn.GetInstance<MainWindowViewModel>().Initialize();

            this.MainWindow = ctn.GetInstance<MainWindow>();
            MainWindow.Show();
        }
    }
}