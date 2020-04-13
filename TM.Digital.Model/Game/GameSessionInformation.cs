using System;
using System.Text;

namespace TM.Digital.Model.Game
{
    public class GameSessionInformation
    {
        public Guid GameSessionId { get; set; }
        public string Owner { get; set; }

        public int NumnerOfPlayers { get; set; }
        public Guid OwnerId { get; set; }
    }
}
