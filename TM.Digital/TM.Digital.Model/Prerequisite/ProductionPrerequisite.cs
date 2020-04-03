using System.Linq;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Prerequisite
{
    //might be used for prerequisite which requires any production or cards which require to downgrade resource production
    public class ProductionPrerequisite : IPrerequisite
    {
        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }


        public bool MatchPrerequisite(Player.Player player, Board.Board board)
        {
            return player.Resources.First(r => r.ResourceType == ResourceType).Production > Amount;
        }
    }
}