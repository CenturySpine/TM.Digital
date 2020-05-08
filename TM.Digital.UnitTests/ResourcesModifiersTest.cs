using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Resources;
using TM.Digital.Services;

namespace TM.Digital.UnitTests
{
    [TestClass]
    public class ResourcesModifiersTest : EffectHandlerTestsBase
    {

        [TestMethod]
        public void BaseTestOne()
        {
            SetupPlayer();
            Player.PlayedCards = new List<Patent>();
            Player.PlayedCards.Add(new Patent()
            {
                MineralModifiers = new MineralModifiers()
                {
                    new MineralModifier(){ResourceType = ResourceType.Steel,Value = 1},new MineralModifier(){ResourceType = ResourceType.Titanium,Value = 1}
                }
            });

            EffectHandler.EvaluateResourceModifiers(Player).GetAwaiter().GetResult();

            Assert.AreEqual(3,Player[ResourceType.Steel].MoneyValueModifier);
            Assert.AreEqual(4, Player[ResourceType.Titanium].MoneyValueModifier);
        }

        [TestMethod]
        public void BaseTestTwo()
        {
            SetupPlayer();
            Player.Corporation = new Corporation()
            {
                Name = "Hellion",
                MineralModifiers = new MineralModifiers()
                {
                    new MineralModifier(){ResourceType = ResourceType.Heat,Value = 1}
                }
            };
            Player.PlayedCards = new List<Patent>();


            EffectHandler.EvaluateResourceModifiers(Player).GetAwaiter().GetResult();

            Assert.AreEqual(2, Player[ResourceType.Steel].MoneyValueModifier);
            Assert.AreEqual(3, Player[ResourceType.Titanium].MoneyValueModifier);
            Assert.AreEqual(1, Player[ResourceType.Heat].MoneyValueModifier);
        }
    }
}
