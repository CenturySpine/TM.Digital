using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
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

            PlayerResource(ResourceType.Money).UnitCount = 0;
            var t = EffectHandler.HandleInitialPatentBuy(Player, new List<Patent>
           {
               new Patent(),
               new Patent()
           },
                new Corporation
                {
                    ResourcesEffects = new List<ResourceEffect>
                {
                new ResourceEffect
                {
                    ResourceType = ResourceType.Money,ResourceKind = ResourceKind.Unit,Amount = 44
                }
             }
                }, BoardGenerator.Instance.Original(), new List<Player>(), new CardDrawer());
            t.Wait();
            Assert.AreEqual(38, PlayerResource(ResourceType.Money).UnitCount);
        }
    }
}