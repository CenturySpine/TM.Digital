using TM.Digital.Model.Board;
using TM.Digital.Model.Player;
using TM.Digital.Model.Tile;
using TM.Digital.Services;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor.Board
{
    public class BoardTesterViewModel : NotifierBase
    {
        private BoardGenerator _boardGen;
        private Model.Board.Board _board;
        private TileEffect _tile;
        private  bool _surroundingTestMode;

        public BoardTesterViewModel()
        {
            _boardGen = BoardGenerator.Instance;
            Board = _boardGen.Original();
            GetAuthorizedPlacesCommand = new RelayCommand(ExecuteGetAuthorizedPlaces);
            Player = ModelFactory.NewPlayer("test", false);
            Newtilecommand = new RelayCommand(ExecuteNewTile);
        }

        private void ExecuteNewTile(object obj)
        {
            Tile = new TileEffect();
        }

        public Player Player { get; set; }

        public TileEffect Tile
        {
            get => _tile;
            set { _tile = value;OnPropertyChanged(); }
        }

        private async void ExecuteGetAuthorizedPlaces(object obj)
        {
            //if(SelectedPlace==null)
            //    return;


            var currentBoard = Board.Clone();
            BoardHandler.PendingTileEffect = Tile;
            var choiceBoard = BoardHandler.GetPlacesChoices(BoardHandler.PendingTileEffect, currentBoard, Player);
            Board = choiceBoard;

            
        }

        public Model.Board.Board Board
        {
            get => _board;
            set { _board = value;OnPropertyChanged(); }
        }

        public RelayCommand GetAuthorizedPlacesCommand { get; }

        public async void SelectPlace(object o)
        {
            
            SelectedPlace = null;
            if (o is BoardPlace bp)
            {
                if (SurroundingTestMode)
                {
                    var currentBoard = Board.Clone();
                    SelectedPlace = bp;
                    var indexes = BoardHandler.GetPlaceSurroundingsIndexes(bp, currentBoard);
                    var bps = BoardHandler.GetTilesFromIndexes(indexes, currentBoard);
                    foreach (var boardPlace in bps)
                    {
                        boardPlace.CanBeChosed = true;
                    }
                    Board = currentBoard;
                }
                else
                {
                    var currentBoard = Board.Clone();
                    await BoardHandler.PlaceTileOnBoard(bp, Player, BoardHandler.PendingTileEffect, currentBoard,
                        new CardDrawer());
                    Board = currentBoard;
                }



            }

            
        }

        public BoardPlace SelectedPlace { get; set; }

        public RelayCommand Newtilecommand { get; }

        public bool SurroundingTestMode
        {
            get => _surroundingTestMode;
            set { _surroundingTestMode = value;OnPropertyChanged(); }
        }
    }
}