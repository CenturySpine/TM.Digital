using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Corporations
{
    public abstract class Corporation: Card
    {
        public int StartingMoney { get; set; }
    }

    public class Prelude : Card
    {
        
    }
}
