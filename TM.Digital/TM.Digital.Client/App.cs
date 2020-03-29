using SimpleInjector;
using System.Windows;
using TM.Digital.Model.Game;

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


    }
}