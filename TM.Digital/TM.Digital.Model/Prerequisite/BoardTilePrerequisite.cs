using System.Linq;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Prerequisite
{
    public class BoardTilePrerequisite : IPrerequisite
    {
        public TileType TileType { get; set; }
        public int Count { get; set; }
        public bool IsMax { get; set; }
        public bool MustBeOwned { get; set; }

        public bool MatchPrerequisite(Player.Player player, Board.Board board)
        {
            var playedTiles = board.BoardLines
                .SelectMany(r => r.BoardPlaces)
                .Concat(board.IsolatedPlaces)
                .Where(t => t.PlayedTile != null && t.PlayedTile.Type == TileType)
                .ToList();

            var playersTiles =
                playedTiles.Where(t => t.PlayedTile.Owner.HasValue && t.PlayedTile.Owner == player.PlayerId);

            if (!MustBeOwned)
                return IsMax ? playedTiles.Count() <= Count : playedTiles.Count() >= Count;

            return IsMax ? playersTiles.Count() <= Count : playersTiles.Count() >= Count;
        }
    }
}