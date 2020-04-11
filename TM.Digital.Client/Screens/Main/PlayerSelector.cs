using System;
using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.ViewModelCore;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Transport;

namespace TM.Digital.Client.Screens.Main
{
    public delegate void PlayerPassedEventHandler(Guid playerid);
    public delegate void PlayerSkipedEventHandler(Guid playerid);
    public class PlayerSelector
    {
        public event PlayerPassedEventHandler PlayerPassed;
        public event PlayerSkipedEventHandler PlayerSkipped;
        public PlayerSelector(Player player, Guid gameId)
        {
            Player = player;
            GameId = gameId;
            PatentsSelectors = new ObservableCollection<PatentSelector>(player.HandCards.Select(c => new PatentSelector { Patent = c }));
            PassCommand = new RelayCommand(ExecutePass, CanExecutePass);
            SkipCommand = new RelayCommand(ExecuteSkip,CanExecuteSkip);
            SelectCardCommand = new RelayCommand(ExecutePlayCard, CanExecutePlayCard);
        }

        private bool CanExecutePass(object arg)
        {
            return Player.RemainingActions == 0 || Player.RemainingActions == 2 ;
        }

        private async void ExecutePlayCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                if (!patent.Patent.CanBePlayed)
                    return;

                await TmDigitalClientRequestHandler.Instance.Post<Patent>($"game/{GameId}/play/{Player.PlayerId}", patent.Patent);
            }
        }

        private bool CanExecutePlayCard(object arg)
        {
            if (arg is PatentSelector patent)
            {
                if (!patent.Patent.CanBePlayed)
                    return false;
                return true;
            }

            return false;
        }

        public RelayCommand SelectCardCommand { get; set; }

        private bool CanExecuteSkip(object arg)
        {
            return Player.RemainingActions == 1;
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
        public Guid GameId { get; }
        public ObservableCollection<PatentSelector> PatentsSelectors { get; set; }

        public RelayCommand PassCommand { get; }

        public RelayCommand SkipCommand { get; }

        protected virtual void OnPlayerSkipped(Guid playerid)
        {
            PlayerSkipped?.Invoke(playerid);
        }

        protected virtual void OnPlyerPassed(Guid playerid)
        {
            PlayerPassed?.Invoke(playerid);
        }
    }
}