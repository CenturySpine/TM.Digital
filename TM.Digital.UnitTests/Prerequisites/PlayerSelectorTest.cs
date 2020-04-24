using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TM.Digital.Cards;
using TM.Digital.Client.Screens.Main;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;
using TM.Digital.Services;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class PlayerSelectorTest 
    {
        private PlayerSelector _playerSelector;

        [TestInitialize]
        public void Setup()
        {
            var patent = PatentFactory.NewPatent();
            patent.BaseCost = patent.ModifiedCost = 34;
            patent.Tags.AddRange(new[] { Tags.Space, Tags.Building });
            var player = ModelFactory.NewPlayer("test", false);
            player.HandCards.Add(patent);
            player[ResourceType.Titanium].UnitCount = 3;
            player[ResourceType.Titanium].UnitCount = 3;
            player[ResourceType.Titanium].MoneyValueModifier = 3;
            player[ResourceType.Steel].MoneyValueModifier = 2;
            _playerSelector = new PlayerSelector(player, Guid.NewGuid());
        }

        [TestMethod]
        public void TestPatentCostBaseNotEnough()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 20;


            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsFalse(baseResult);
        }

        [TestMethod]
        public void TestPatentCostBaseEnough()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 35;
            
            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsTrue(baseResult);
        }

        [TestMethod]
        public void TestPatentCostBaseEnoughWithModifiers()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 20;


            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[0].UnitsUsed = 3;//+9
            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[1].UnitsUsed = 3;//+6
            //total value should be 20 + 9 + 6 = 35

            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsTrue(baseResult);
        }

        [TestMethod]
        public void TestPatentCostBaseNotEnoughWithModifiers()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 20;


            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[0].UnitsUsed = 2;//+6
            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[1].UnitsUsed = 3;//+6
            //total value should be 20 + 6 + 6 = 32

            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsFalse(baseResult);
        }
    }
}