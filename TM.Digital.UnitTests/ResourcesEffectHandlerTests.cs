using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Board;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services;

namespace TM.Digital.UnitTests
{
    [TestClass]
    public class ResourcesEffectHandlerTests : EffectHandlerTestsBase
    {
        private Board _board;


        [TestInitialize]
        public void Setup()
        {
            SetupPlayer();
            _board = BoardGenerator.Instance.BoardShell();
        }
        [TestMethod]
        public void BasicResourcesUnitEffect()
        {
            var resEff = new ResourceEffect
            { Amount = 3, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Energy };

            EffectHandler.HandleResourceEffect(Player, resEff, new List<Player>(), _board, new CardDrawer(), null).GetAwaiter().GetResult();

            Assert.AreEqual(8, PlayerResource(ResourceType.Energy).UnitCount);
        }

        [TestMethod]
        public void BasicResourcesProductionEffect()
        {
            var resEff = new ResourceEffect
            { Amount = 3, ResourceKind = ResourceKind.Production, ResourceType = ResourceType.Energy };

            EffectHandler.HandleResourceEffect(Player, resEff, new List<Player>(), _board, new CardDrawer(), null).GetAwaiter().GetResult();

            Assert.AreEqual(5, PlayerResource(ResourceType.Energy).Production);
        }
        [TestMethod]
        public void ResourcesProductionBelowZeroProductionEffect()
        {
            var resEff = new ResourceEffect
            { Amount = -3, ResourceKind = ResourceKind.Production, ResourceType = ResourceType.Energy };

            EffectHandler.HandleResourceEffect(Player, resEff, new List<Player>(), _board, new CardDrawer(), null).GetAwaiter().GetResult();

            Assert.AreEqual(0, PlayerResource(ResourceType.Energy).Production);
        }
        [TestMethod]
        public void ResourcesProductionBelowZeroMoneyProductionEffect()
        {
            var resEff = new ResourceEffect
            { Amount = -2, ResourceKind = ResourceKind.Production, ResourceType = ResourceType.Money };

            EffectHandler.HandleResourceEffect(Player, resEff, new List<Player>(), _board, new CardDrawer(), null).GetAwaiter().GetResult();

            Assert.AreEqual(-1, PlayerResource(ResourceType.Money).Production);
        }
        [TestMethod]
        public void ResourcesProductionBelowZeroUnitEffect()
        {
            var resEff = new ResourceEffect
            { Amount = -6, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Energy };

            EffectHandler.HandleResourceEffect(Player, resEff, new List<Player>(), _board, new CardDrawer(), null).GetAwaiter().GetResult();

            Assert.AreEqual(0, PlayerResource(ResourceType.Energy).UnitCount);
        }

    }
}
