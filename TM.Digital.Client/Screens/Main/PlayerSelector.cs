using System;
using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model.Player;

namespace TM.Digital.Client.Screens.Main
{
    public delegate void PlayerPassedEventHandler(Guid playerid);
    public delegate void PlayerSkipedEventHandler(Guid playerid);
    public class PlayerSelector
    {
        public event PlayerPassedEventHandler PlyerPassed;
        public event PlayerSkipedEventHandler PlayerSkipped;
        public PlayerSelector(Player player)
        {
            Player = player;
            PatentsSelectors = new ObservableCollection<PatentSelector>(player.HandCards.Select(c => new PatentSelector { Patent = c }));
            PassCommand = new RelayCommand(ExecutePass);
            SkipCommand = new RelayCommand(ExecuteSkip);
        }

        private void ExecuteSkip(object obj)
        {
            OnPlayerSkipped(Player.PlayerId);
        }

        private void ExecutePass(object obj)
        {
            OnPlyerPassed(Player.PlayerId);
        }

        public Player Player { get; set; }
        public ObservableCollection<PatentSelector> PatentsSelectors { get; set; }

        public RelayCommand PassCommand { get; }

        public RelayCommand SkipCommand { get; }

        protected virtual void OnPlayerSkipped(Guid playerid)
        {
            PlayerSkipped?.Invoke(playerid);
        }

        protected virtual void OnPlyerPassed(Guid playerid)
        {
            PlyerPassed?.Invoke(playerid);
        }
    }
}