using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;
using TM.Digital.Services;

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
            EffectHandler.HandleInitialPatentBuy(Player, new List<Patent> { new Patent(), new Patent() }, new Corporation { ResourcesEffects = new List<ResourceEffect>()
            {
                new ResourceEffect()
                {
                    ResourceType = ResourceType.Money,ResourceKind = ResourceKind.Unit,Amount = 44
                }
            }});

            Assert.AreEqual(44,PlayerResource(ResourceType.Money).UnitCount);
        }
    }
}