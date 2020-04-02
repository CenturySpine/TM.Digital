using System;
using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Model.Game
{
    public class GameSetup
    {
        public List<Corporation> Corporations { get; set; }
        
        public Guid PlayerId { get; set; }

        public List<Patent> Patents { get; set; }
    }
}