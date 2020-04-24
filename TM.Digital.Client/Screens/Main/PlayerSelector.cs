using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Transport;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Client.Screens.Main
{
    public delegate void PlayerPassedEventHandler(Guid playerid);

    public delegate void PlayerSkippedEventHandler(Guid playerid);

    public sealed class PlayerSelector
    {
        public event PlayerPassedEventHandler PlayerPassed;

        public event PlayerSkippedEventHandler PlayerSkipped;

        public PlayerSelector(Player player, Guid gameId)
        {
            Player = player;
            GameId = gameId;
            PatentsSelectors = new ObservableCollection<PatentSelector>(ConvertPatentsToSelector(player));
            PassCommand = new RelayCommand(ExecutePass, CanExecutePass);
            SkipCommand = new RelayCommand(ExecuteSkip, CanExecuteSkip);
            SelectCardCommand = new RelayCommand(ExecutePlayCard, CanExecutePlayCard);
        }

        private static IEnumerable<PatentSelector> ConvertPatentsToSelector(Player player)
        {
            return player.HandCards.Select(c =>
            {
                var titaniumUnits = player.Resources.First(r => r.ResourceType == ResourceType.Titanium).UnitCount;
                var steelUnits = player.Resources.First(r => r.ResourceType == ResourceType.Steel).UnitCount;

                var patentSelect = new PatentSelector
                {
                    Patent = c,
                };

                if (c.Tags.Contains(Tags.Space))
                {
                    var modSpace = new MineralsPatentModifier()
                    {
                        ResourceType = ResourceType.Titanium,
                        UnitsUsed = 0,
                        MaxUsage = titaniumUnits
                    };
                    patentSelect.MineralsPatentModifiersSummary.MineralsPatentModifier.Add(modSpace);
                }
                if (c.Tags.Contains(Tags.Building))
                {
                    var modSteel = new MineralsPatentModifier()
                    {
                        ResourceType = ResourceType.Steel,
                        UnitsUsed = 0,
                        MaxUsage = steelUnits
                    };
                    patentSelect.MineralsPatentModifiersSummary.MineralsPatentModifier.Add(modSteel);
                }

                return patentSelect;
            });
        }

        private bool CanExecutePass(object arg)
        {
            return Player.RemainingActions == 0 || Player.RemainingActions == 2;
        }

        private async void ExecutePlayCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                if (!RealCanBePlayedPatent(patent))
                    return;

                ActionPlay action = new ActionPlay
                {
                    Patent = patent.Patent,
                    ResourcesUsages = new List<ActionPlayResourcesUsage>(
                        patent.MineralsPatentModifiersSummary.MineralsPatentModifier.Where(r => r.UnitsUsed > 0)
                            .Select(t => new ActionPlayResourcesUsage()
                            {
                                ResourceType = t.ResourceType, UnitPlayed = t.UnitsUsed,
                            }))
                };

                await TmDigitalClientRequestHandler.Instance.Post<ActionPlay>($"game/{GameId}/play/{Player.PlayerId}", action);
            }
        }

        private bool CanExecutePlayCard(object arg)
        {
            if (arg is PatentSelector patent)
            {
                return RealCanBePlayedPatent(patent);
            }

            return false;
        }

        private bool RealCanBePlayedPatent(PatentSelector patent)
        {
            var rawCostMAtch = //raw cost inferior to players money
                (patent.Patent.ModifiedCost <=
                 Player[ResourceType.Money].UnitCount);

            var mineralUsageMatch = CheckSteelUsage(
                patent.MineralsPatentModifiersSummary,

                patent.Patent.ModifiedCost,
                Player[ResourceType.Money].UnitCount,
                Player[ResourceType.Steel], Player[ResourceType.Titanium]);

            return patent.Patent.CanBePlayed
                   &&
                   rawCostMAtch
                //or minerals units sufficient to play part of the patent
                || mineralUsageMatch


                ;
        }

        private bool CheckSteelUsage(MineralsPatentModifiersSummary patentMineralsPatentModifier,
            int patentModifiedCost, in int totalMoney, params ResourceHandler[] resourceHandlers)
        {
            int finalMineralValue = 0;
            foreach (var resourceHandler in resourceHandlers)
            {
                var mineralModifier = patentMineralsPatentModifier.MineralsPatentModifier.FirstOrDefault(t => t.ResourceType == resourceHandler.ResourceType);
                if (mineralModifier == null) finalMineralValue += 0;
                else
                {
                    finalMineralValue += mineralModifier.UnitsUsed * resourceHandler.MoneyValueModifier;
                   

                }
            }

            patentMineralsPatentModifier.ModifiedRessourceCost = patentModifiedCost - finalMineralValue;

            return finalMineralValue + totalMoney >= patentModifiedCost;
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

        private void OnPlayerSkipped(Guid playerid)
        {
            PlayerSkipped?.Invoke(playerid);
        }

        private void OnPlyerPassed(Guid playerid)
        {
            PlayerPassed?.Invoke(playerid);
        }
    }
}