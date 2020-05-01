using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.Services;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Transport;
using TM.Digital.Ui.Resources.ViewModelCore;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.Client.Screens.Main
{
    public sealed class PlayerSelector : NotifierBase
    {
        private readonly IApiProxy _apiProxy;
        private Player _player;

        public PlayerSelector(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
            PatentsSelectors = new ObservableCollection<PatentSelector>();
            PassCommand = new RelayCommand(ExecutePass, CanExecutePass);
            SkipCommand = new RelayCommand(ExecuteSkip, CanExecuteSkip);
            SelectCardCommand = new RelayCommand(ExecutePlayCard);
            VerifyCardCommand = new RelayCommand(ExecuteVerify);
            ConvertCommand = new RelayCommand(ExecuteConvert);
            ExecuteActionCommand = new RelayCommand(ExecuteBoardAction);
        }

        public RelayCommand ConvertCommand { get; set; }

        public RelayCommand ExecuteActionCommand { get; set; }

        public RelayCommand PassCommand { get; }

        public ObservableCollection<PatentSelector> PatentsSelectors { get; set; }

        public Player Player
        {
            get => _player;
            set { _player = value; OnPropertyChanged(); }
        }

        public RelayCommand SelectCardCommand { get; set; }

        public RelayCommand SkipCommand { get; }

        public RelayCommand VerifyCardCommand { get; set; }

        public void Clean()
        {
            foreach (var patentsSelector in PatentsSelectors)
            {
                if (patentsSelector.MineralsPatentModifiersSummary != null &&
                    patentsSelector.MineralsPatentModifiersSummary.MineralsPatentModifier.Any())
                {
                    foreach (var mineralsPatentModifier in patentsSelector.MineralsPatentModifiersSummary.MineralsPatentModifier)
                    {
                        mineralsPatentModifier.UnitUsedChanged -= ModSpace_UnitUsedChanged;
                    }
                }
            }
        }

        public void Update(Player player)
        {
            Player = player;
            PatentsSelectors.Clear();
            foreach (var patentSelector in ConvertPatentsToSelector(player))
            {
                PatentsSelectors.Add(patentSelector);
            }
        }

        private bool CanExecutePass(object arg)
        {
            return Player != null && (Player.RemainingActions == 0 || Player.RemainingActions == 2);
        }


        private bool CanExecuteSkip(object arg)
        {
            return Player != null && Player.RemainingActions == 1;
        }

        private IEnumerable<PatentSelector> ConvertPatentsToSelector(Player player)
        {
            return player.HandCards.Select(c =>
            {
                var titaniumUnits = player.Resources.First(r => r.ResourceType == ResourceType.Titanium).UnitCount;
                var steelUnits = player.Resources.First(r => r.ResourceType == ResourceType.Steel).UnitCount;

                var patentSelect = new PatentSelector
                {
                    Patent = c,
                };

                if (c.Tags != null && c.Tags.Contains(Tags.Space))
                {
                    var modSpace = new MineralsPatentModifier
                    {
                        ResourceType = ResourceType.Titanium,
                        UnitsUsed = c.TitaniumUnitUsed,
                        MaxUsage = titaniumUnits
                    };
                    modSpace.UnitUsedChanged += ModSpace_UnitUsedChanged;
                    patentSelect.MineralsPatentModifiersSummary.MineralsPatentModifier.Add(modSpace);
                }
                if (c.Tags != null && c.Tags.Contains(Tags.Building))
                {
                    var modSteel = new MineralsPatentModifier
                    {
                        ResourceType = ResourceType.Steel,
                        UnitsUsed = c.SteelUnitUsed,
                        MaxUsage = steelUnits
                    };
                    modSteel.UnitUsedChanged += ModSpace_UnitUsedChanged;
                    patentSelect.MineralsPatentModifiersSummary.MineralsPatentModifier.Add(modSteel);
                }

                return patentSelect;
            });
        }

        private async Task CurrentPlayer_PlayerSkipped()
        {
            await _apiProxy.Skip();

        }

        private async Task CurrentPlayerPlayerPassed()
        {
            await _apiProxy.Pass();

        }

        private async void ExecuteBoardAction(object obj)
        {
            if (obj is BoardAction boardAction)
            {
                await _apiProxy.PlayBoardAction(boardAction);
            }
            if (obj is Action cardAction)
            {
                await _apiProxy.PlayCardAction(cardAction);
            }
        }

        private async void ExecuteConvert(object obj)
        {
            if (obj is ResourceHandler rh)
            {
                await _apiProxy.PlayConvertResourcesAction(rh);
            }
        }

        private async void ExecutePass(object obj)
        {
            await CurrentPlayerPlayerPassed();
        }

        private async void ExecutePlayCard(object obj)
        {
            if (obj is PatentSelector patent)
            {
                //if (!RealCanBePlayedPatent(patent))
                //    return;

                CardActionPlay cardAction = new CardActionPlay
                {
                    Patent = patent.Patent,
                    ResourcesUsages = new List<ActionPlayResourcesUsage>(
                        patent.MineralsPatentModifiersSummary.MineralsPatentModifier.Where(r => r.UnitsUsed > 0)
                            .Select(t => new ActionPlayResourcesUsage
                            {
                                ResourceType = t.ResourceType,
                                UnitPlayed = t.UnitsUsed,
                            }))
                };
                await _apiProxy.PlayCard(cardAction);
            }
        }

        private async void ExecuteSkip(object obj)
        {
            await CurrentPlayer_PlayerSkipped();
        }
        private async void ExecuteVerify(object obj)
        {
            if (obj is PatentSelector patent)
            {
                await _apiProxy.VerifyCardPlayability(patent);
            }
        }
        private async void ModSpace_UnitUsedChanged(object sender, EventArgs e)
        {
            if (sender is MineralsPatentModifier)
            {
                var targetPatent = PatentsSelectors.FirstOrDefault(ps =>
                    ps.MineralsPatentModifiersSummary.MineralsPatentModifier.Contains(sender as MineralsPatentModifier));

                if (targetPatent != null)
                {
                    var mod = new PlayCardWithResources()
                    {
                        Patent = targetPatent.Patent,
                        CardMineralModifiers = targetPatent.MineralsPatentModifiersSummary.MineralsPatentModifier
                            .Select(m => new ActionPlayResourcesUsage
                            {
                                ResourceType = m.ResourceType,
                                UnitPlayed = m.UnitsUsed
                            }).ToList()
                    };
                    await _apiProxy.VerifyResourcesModifiedCostCardPlayability(mod);

                }
            }
        }
    }
}