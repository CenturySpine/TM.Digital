using System.ComponentModel;

namespace TM.Digital.Model.Cards
{
    public class Patent : Card
    {
        [Category("Description")]
        public int BaseCost { get; set; }

        [Browsable(false)]
        public int ModifiedCost { get; set; }

        [Category("Description")]
        public Prerequisites Prerequisites { get; set; }

        [Browsable(false)]
        public bool CanBePlayed { get; set; }

        [Browsable(false)]
        public int TitaniumUnitUsed { get; set; }

        [Browsable(false)]
        public int SteelUnitUsed { get; set; }

        [Browsable(false)]
        public bool IsSameCost
        {
            get { return BaseCost == ModifiedCost; }
        }
    }
}