using TM.Digital.Model.Board;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Cards
{
    public class ActionTo
    {
        public ActionTarget ActionTarget { get; set; }

        public ResourceType ResourceType { get; set; }

        public BoardLevelType BoardLevelType { get; set; }

        public int Amount { get; set; }
        public ResourceKind ResourceKind { get; set; }


        public TileEffect TileEffect { get; set; }
    }
}