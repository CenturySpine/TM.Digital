using System;
using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Model.Game
{
    public class GameSetupSelection
    {
        public Guid GameId { get; set; }
        public Dictionary<string, bool> Corporation { get; set; }
        public Guid PlayerId { get; set; }

        public Dictionary<string, bool> BoughtCards { get; set; }
        
    }
}