using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Model.Player;

namespace TM.Digital.Client
{
    public class PlayerSelector
    {
        public PlayerSelector(Player player)
        {
            Player = player;
            PatentsSelectors = new ObservableCollection<PatentSelector>(player.HandCards.Select(c => new PatentSelector { Patent = c }));
        }

        public Player Player { get; set; }
        public ObservableCollection<PatentSelector> PatentsSelectors { get; set; }
    }
}