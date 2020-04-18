using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Cards;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class PatentCostTests
    {
        private PatentCostPrerequisite _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new PatentCostPrerequisite();
        }

        [TestMethod]
        public void TestBaseCostWithoutModifiersNotEnoughMoney()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 4;

            Patent card = PatentFactory.NewPatent();
            card.ModifiedCost = 12;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsFalse(canPlay);
        }

        [TestMethod]
        public void TestBaseCostWithoutModifiersEnoughMoney()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 15;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 4;

            Patent card = PatentFactory.NewPatent();
            card.ModifiedCost = 12;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithBuildingTagsAdnSteelEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 15;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 4;


            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Building);
            card.ModifiedCost = 18;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithBuildingTagsAdnSteelNotEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 15;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 4;


            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Building);
            card.ModifiedCost = 20;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsFalse(canPlay);
        }

        [TestMethod]
        public void TestCostWithSpaceTagsAdnTitaniumNotEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 3;


            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = 20;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsFalse(canPlay);
        }


        [TestMethod]
        public void TestCostWithSpaceTagsAdnTitaniumEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 4;


            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = 20;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithSpaceTagsAdnTitaniumEnoughWithTitaniumModifierCard()
        {
            Player p = ModelFactory.NewPlayer("toto", false);
            var playedPatent = PatentFactory.NewPatent();
            playedPatent.MineralModifiers = new MineralModifiers()
            {
                TitaniumModifier = new MineralModifier()
                {
                    ResourceType = ResourceType.Titanium,
                    Value = 1
                }
            };
            p.PlayedCards.Add(playedPatent);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 3;


            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = 20;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }


    }
}
