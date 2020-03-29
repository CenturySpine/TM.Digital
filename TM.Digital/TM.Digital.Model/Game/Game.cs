using System;
using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Model.Game
{
    public class Game
    {
    }

    public class GameSetup
    {
        public List<Corporation> Corporations { get; set; } = new List<Corporation>();
        public List<Card> Cards { get; set; } = new List<Card>();

        public List<Prelude> Preludes { get; set; } = new List<Prelude>();
        public Guid PlayerId { get; set; }
    }
    public class GameSetupSelection
    {
        public Corporation Corporation { get; set; }
        public Guid PlayerId { get; set; }
    }
}