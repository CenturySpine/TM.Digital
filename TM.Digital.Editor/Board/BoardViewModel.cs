using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TM.Digital.Model;
using TM.Digital.Model.Board;
using TM.Digital.Model.Player;
using TM.Digital.Services;
using TM.Digital.Services.Common;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor.Board
{
    public class BoardViewModel : PackManagerViewModelBase
    {
        private BoardGenerator _boardGen;
        private object _selectedObject;

        //private TileEffect _tile;
        //private bool _surroundingTestMode;

        //private string _boardName;
        private ObservableCollection<Model.Board.Board> _allBoards;
        private CardReferencesHolder packsData;
        private BoardPlace _copiedTile;

        public BoardViewModel()
        {
            _boardGen = BoardGenerator.Instance;
            SelectedObject = _boardGen.Original();

            //GetAuthorizedPlacesCommand = new RelayCommand(ExecuteGetAuthorizedPlaces);
            Player = ModelFactory.NewPlayer("test", false);
            AllBoards = new ObservableCollection<Model.Board.Board>();
            SavBoardCommand = new RelayCommand(ExecuteSaveBoard);
            LoadCommand = new RelayCommand(ExecuteLoad);
            NewBoardCommand = new RelayCommand(ExecuteNewBoard);
            DeleteBoardCommand = new RelayCommand(ExecuteDelete);
            SelectBoardPlace = new RelayCommand(ExecuteSelectPlace);
            SelectBoardCommand = new RelayCommand(ExecuteSelectBoard);
            //Newtilecommand = new RelayCommand(ExecuteNewTile);
            IsEditor = true;
            CopyCommand = new RelayCommand(ExecuteCopy);
            PasteCommand = new RelayCommand(ExecutePaste, CanExecutePaste);
        }

        private bool CanExecutePaste(object arg)
        {
            return _copiedTile != null;
        }

        public RelayCommand PasteCommand { get; set; }

        private void ExecutePaste(object obj)
        {
            if (obj is BoardPlace bp && _copiedTile != null)
            {

                bp.CopyFrom(_copiedTile);
                
                Refresh();
            }
        }

        public RelayCommand CopyCommand { get; set; }

        private void ExecuteCopy(object obj)
        {
            if (obj is BoardPlace bp)
            {
                _copiedTile = bp.Clone();
            }

        }

        private void ExecuteSelectBoard(object obj)
        {
            if (obj is Model.Board.Board board)
            {
                SelectedObject = board;
            }

        }

        public bool IsEditor { get; set; }

        private void ExecuteSelectPlace(object obj)
        {
            if (obj is BoardPlace bp)
            {
                SelectedObject = bp;
            }
        }

        public RelayCommand SelectBoardPlace { get; set; }

        private void ExecuteDelete(object obj)
        {
            if (SelectedObject == null || !(SelectedObject is Model.Board.Board b)) return;
            if (MessageBox.Show($"Delete board '{b?.Name}' ?", "confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AllBoards.Remove(b);
            }
        }

        private void ExecuteNewBoard(object obj)
        {
            var b = BoardGenerator.Instance.Original();

            AllBoards.Add(b);
            SelectedObject = SelectedObject;
        }

        private async void ExecuteLoad(object obj)
        {
            packsData = await Load();
            if (packsData.Boards != null && packsData.Boards.Any())
            {
                AllBoards.Clear();
                foreach (var VARIABLE in packsData.Boards

                )
                {
                    AllBoards.Add(VARIABLE);
                }

                if (AllBoards.Any())
                {
                    SelectedObject = AllBoards.First();
                }
            }
        }

        private async void ExecuteSaveBoard(object obj)
        {

            if (SelectedObject is Model.Board.Board board)
            {


                packsData.Boards = new List<Model.Board.Board>(AllBoards.Select(f => f));
                Save(packsData);
            }
            else
            {
                MessageBox.Show("Board is not selected. Please click the 'Select' button near the board list picker");
            }
        }

        //private void ExecuteNewTile(object obj)
        //{
        //    Tile = new TileEffect();
        //}

        public Player Player { get; set; }

        //public TileEffect Tile
        //{
        //    get => _tile;
        //    set { _tile = value; OnPropertyChanged(); }
        //}

        //private async void ExecuteGetAuthorizedPlaces(object obj)
        //{
        //    //if(SelectedPlace==null)
        //    //    return;

        //    var currentBoard = Board.Clone();
        //    BoardTilesHandler.PendingTileEffect = Tile;
        //    var choiceBoard = BoardTilesHandler.GetPlacesChoices(BoardTilesHandler.PendingTileEffect, currentBoard, Player);
        //    Board = choiceBoard;

        //}

        public object SelectedObject
        {
            get => _selectedObject;
            set { _selectedObject = value; OnPropertyChanged(); }
        }

        //public RelayCommand GetAuthorizedPlacesCommand { get; }

        //public async void SelectPlace(object o)
        //{
        //    SelectedPlace = null;
        //    if (o is BoardPlace bp)
        //    {
        //        if (SurroundingTestMode)
        //        {
        //            var currentBoard = Board.Clone();
        //            SelectedPlace = bp;
        //            var indexes = BoardTilesHandler.GetPlaceSurroundingsIndexes(bp, currentBoard);
        //            var bps = BoardTilesHandler.GetTilesFromIndexes(indexes, currentBoard);
        //            foreach (var boardPlace in bps)
        //            {
        //                boardPlace.CanBeChoosed = true;
        //            }
        //            Board = currentBoard;
        //        }
        //        else
        //        {
        //            var currentBoard = Board.Clone();
        //            await BoardTilesHandler.PlaceTileOnBoard(bp, Player, BoardTilesHandler.PendingTileEffect, currentBoard,
        //                new CardDrawer());
        //            Board = currentBoard;
        //        }

        //    }

        //}

        //public BoardPlace SelectedPlace { get; set; }

        //public RelayCommand Newtilecommand { get; }

        //public bool SurroundingTestMode
        //{
        //    get => _surroundingTestMode;
        //    set { _surroundingTestMode = value; OnPropertyChanged(); }
        //}

        //public string BoardName
        //{
        //    get => _boardName;
        //    set { _boardName = value; OnPropertyChanged(); }
        //}

        public RelayCommand SavBoardCommand { get; set; }

        public RelayCommand LoadCommand { get; set; }

        public ObservableCollection<Model.Board.Board> AllBoards
        {
            get => _allBoards;
            set { _allBoards = value; OnPropertyChanged(); }
        }

        public RelayCommand NewBoardCommand { get; set; }

        public RelayCommand DeleteBoardCommand { get; set; }

        public RelayCommand SelectBoardCommand { get; }

        public void Refresh()
        {



                OnPropertyChanged(nameof(SelectedObject));
            
        }
    }
}