using System.Collections.Generic;
using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Game
{
    public class Game
    {
        public Board.Board Board { get; set; }

        public List<Player.Player> AllPlayers { get; set; }
    }
}