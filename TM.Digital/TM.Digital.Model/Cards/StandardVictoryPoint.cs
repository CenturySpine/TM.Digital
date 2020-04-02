namespace TM.Digital.Model.Cards
{
    public class StandardVictoryPoint : ICardVictoryPoints
    {
        public int Points { get; set; }
        public int VictoryPoint(Card card)
        {
            return Points;
        }
    }
}