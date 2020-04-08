using System;
using System.Threading.Tasks;
using SimpleInjector;
using System.Windows;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Transport;

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
            ctn.Register<MainMenuViewModel>(Lifestyle.Singleton);
            ctn.Register<WaitingGameScreenViewModel>(Lifestyle.Singleton);
            ctn.Register<IApiProxy,ApiProxy>(Lifestyle.Singleton);

            ctn.Register<PopupService>(Lifestyle.Singleton);

            //resolve
            await ctn.GetInstance<MainWindowViewModel>().Initialize();

            this.MainWindow = ctn.GetInstance<MainWindow>();
            MainWindow.Show();
        }
    }

    public class ApiProxy : IApiProxy
    {
        public async Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<Player>($"game/join/{selectedSessionGameSessionId}/{playerName}");
        }

        public async Task<GameSessions> GetGameSessions()
        {
            return await TmDigitalClientRequestHandler.Instance.Request<GameSessions>("game/sessions");
        }

        public async Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers)
        {
            return await TmDigitalClientRequestHandler.Instance.Request<GameSessionInformation>($"game/create/{playerName}/{numberOfPlayers}" );
        }
    }

    public interface IApiProxy
    {
        Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName);
        Task<GameSessions> GetGameSessions();
        Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers);
    }
}