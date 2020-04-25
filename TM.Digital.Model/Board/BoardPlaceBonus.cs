using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Board
{
    public class BoardPlaceBonus
    {
        public ResourceType BonusType { get; set; }
        
        public ResourceKind BonusKind { get; set; }

        public BoardPlaceBonus Clone()
        {
            return new BoardPlaceBonus()
            {
                BonusKind = BonusKind,
                BonusType = BonusType,
            };
        }
    }
}