using SimpleInjector;
using System.Windows;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.Screens.Main;
using TM.Digital.Client.Screens.Menu;
using TM.Digital.Client.Screens.Wait;
using TM.Digital.Client.Services;

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

            //ctn.Register<GameSetupViewModel>(Lifestyle.Transient);
            ctn.Register<MainMenuViewModel>(Lifestyle.Singleton);
            ctn.Register<WaitingGameScreenViewModel>(Lifestyle.Singleton);
            ctn.Register<ActionChoiceViewModel>(Lifestyle.Singleton);
            ctn.Register<GameSetupViewModel>(Lifestyle.Singleton);
            ctn.Register<IApiProxy, ApiProxy>(Lifestyle.Singleton);
            ctn.Register<LoggerViewModel>(Lifestyle.Singleton);
            ctn.Register<GameUpdateService>(Lifestyle.Singleton);
            ctn.Register<BoardViewModel>(Lifestyle.Singleton);
            ctn.Register<PlayerSelector>(Lifestyle.Singleton);

            //ctn.Register<PopupService>(Lifestyle.Singleton);

            //resolve
            await ctn.GetInstance<MainWindowViewModel>().Initialize();

            this.MainWindow = ctn.GetInstance<MainWindow>();
            MainWindow.Show();
        }
    }
}