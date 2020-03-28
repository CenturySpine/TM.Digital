using System.Threading.Tasks;
using TM.Digital.Model.Board;
using TM.Digital.Transport;

namespace TM.Digital.Client
{
    public class MainWindowViewModel : NotifierBase
    {

        private Board _board;

        public Board Board
        {
            get => _board;
            set { _board = value;OnPropertyChanged(nameof(Board)); }
        }

        public async Task Initialize()
        {
            Refresh = new RelayCommand(ExecuteRefresh);
            await GetBoard();
        }

        private async Task GetBoard()
        {
            Board = await TmDigitalClientRequestHandler.Instance.Request<Board>("marsboard/original");
        }

        private async void ExecuteRefresh(object obj)
        {
            await GetBoard();
        }

        public RelayCommand Refresh { get; set; }
    }
}