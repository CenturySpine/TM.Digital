using System;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Model.Game
{
    public class GameSetupSelection
    {
        public Corporation Corporation { get; set; }
        public Guid PlayerId { get; set; }
    }
}