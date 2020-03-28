namespace TM.Digital.Model.Board
{
    public class BoardPlaceReservedSpace
    {
        public ReservedFor ReservedFor { get; set; }
        public bool IsExclusive { get; set; } = true;
    }
}