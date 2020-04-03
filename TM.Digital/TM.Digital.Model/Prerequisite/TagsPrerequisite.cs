using System.Linq;
using TM.Digital.Model.Cards;

namespace TM.Digital.Model.Prerequisite
{
    public class TagsPrerequisite : IPrerequisite
    {
        public Tags Tag { get; set; }
        public int Count { get; set; }

        
        public bool MatchPrerequisite(Player.Player player, Board.Board board)
        {
            return player.HandCards.SelectMany(c => c.Tags).Count(t => t == Tag) >= Count;
        }
    }
}