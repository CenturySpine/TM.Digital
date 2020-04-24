namespace TM.Digital.Model.Cards
{
    public class Patent : Card
    {
        public int BaseCost { get; set; }
        public int ModifiedCost { get; set; }

        public Prerequisites Prerequisites { get; set; }
        public bool CanBePlayed { get; set; }

        public bool IsSameCost
        {
            get { return BaseCost == ModifiedCost; }
        }
    }
}