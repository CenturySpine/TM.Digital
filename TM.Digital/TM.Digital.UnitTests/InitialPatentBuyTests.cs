using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.UnitTests
{
    [TestClass]
    public class InitialPatentBuyTests : EffectHandlerTestsBase
    {


        [TestInitialize]
        public void Setup()
        {
            SetupPlayer();
        }

        [TestMethod]
        public void BasicPatentBuyTest()

        {
            EffectHandler.HandleInitialPatentBuy(Player, new List<Patent>() { new Patent(), new Patent() }, new Corporation() { StartingMoney = 50 });

            Assert.AreEqual(44,PlayerResource(ResourceType.Money).UnitCount);
        }
    }
}