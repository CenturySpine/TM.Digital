using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Model
{
    public class ExtensionPack
    {
        public Extensions Name { get; set; }

        public List<Patent> Patents { get; set; }
        public List<Corporation> Corporations { get; set; }

        public List<Prelude> Preludes { get; set; }
    }
}