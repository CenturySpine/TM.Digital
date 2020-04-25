namespace TM.Digital.Model.Board
{
    public class BoardPlaceReservedSpace
    {
        public ReservedFor ReservedFor { get; set; }
        public bool IsExclusive { get; set; } = true;

        public BoardPlaceReservedSpace Clone()
        {
            return  new BoardPlaceReservedSpace
            {
                IsExclusive = IsExclusive,
                ReservedFor = ReservedFor
            };
        }
    }
}