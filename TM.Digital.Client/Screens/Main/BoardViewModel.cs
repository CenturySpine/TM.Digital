using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Services;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Main
{
    public class BoardViewModel : NotifierBase, IComponentConfigurable
    {
        private readonly IApiProxy _apiProxy;
        private Board _board;
        private RelayCommand _selectBoardPlace;


        public RelayCommand SelectBoardPlace
        {
            get => _selectBoardPlace;
            set { _selectBoardPlace = value; OnPropertyChanged(nameof(SelectBoardPlace)); }
        }
        public BoardViewModel(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
            SelectBoardPlace = new RelayCommand(ExecuteSelectBoardPlace, CanExecuteSelectBoardPlace);
        }
        private bool CanExecuteSelectBoardPlace(object arg)
        {
            if (arg is BoardPlace bp)
            {
                return bp.CanBeChosed;
            }

            return false;
        }
        private async void ExecuteSelectBoardPlace(object obj)
        {
            if (obj is BoardPlace bp)
            {
                await _apiProxy.PlaceTile(bp);
            }
        }
        public Board Board
        {
            get { return _board; }
            set { _board = value; OnPropertyChanged(); }
        }
        public void RegisterSubscriptions(HubConnection hubConnection)
        {
            hubConnection.On<string, string>(ServerPushMethods.PlaceTileRequest, (user, message) =>
            {
                if (Guid.Parse(user) == GameData.PlayerId)
                {
                    ShowTilesPlacement(message);
                }
            });
        }
        private void ShowTilesPlacement(string message)
        {
            var gameResult2 = JsonConvert.DeserializeObject<Board>(message);
            Board = gameResult2;
            CommandManager.InvalidateRequerySuggested();
        }
        public async Task GetBoard()
        {
            Board = await _apiProxy.GetBoard();

        }
        public void Update(Board gameResult2Board)
        {
            Board = gameResult2Board;
        }
    }
}