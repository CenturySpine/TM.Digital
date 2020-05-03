using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
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

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 4;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 3;

            Patent card = PatentFactory.NewPatent();
            card.ModifiedCost = card.BaseCost = 12;

            PrerequisiteHandler.VerifyResourcesUsage(card, new List<ActionPlayResourcesUsage>(), p);

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsFalse(canPlay);
        }

        [TestMethod]
        public void TestBaseCostWithoutModifiersEnoughMoney()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 15;

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 4;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 3;

            Patent card = PatentFactory.NewPatent();
            card.ModifiedCost = card.BaseCost = 12;

            PrerequisiteHandler.VerifyResourcesUsage(card, new List<ActionPlayResourcesUsage>(), p);

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithBuildingTagsAdnSteelEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 15;

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 4;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 3;

            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Building);
            card.BaseCost = 18;

            PrerequisiteHandler.VerifyResourcesUsage(card, new List<ActionPlayResourcesUsage>
            {new ActionPlayResourcesUsage
                {
                        ResourceType = ResourceType.Steel,UnitPlayed = 2,
                    },
            }, p);

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithBuildingTagsAdnSteelNotEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 15;

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 4;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 3;

            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Building);
            card.BaseCost = card.ModifiedCost = 20;

            PrerequisiteHandler.VerifyResourcesUsage(card, new List<ActionPlayResourcesUsage>
            {new ActionPlayResourcesUsage
                {
                    ResourceType = ResourceType.Steel,UnitPlayed = 2,
                },

            }, p);
            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsFalse(canPlay);
        }

        [TestMethod]
        public void TestCostWithSpaceTagsAdnTitaniumNotEnough()
        {
            Player p = ModelFactory.NewPlayer("toto", false);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 3;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 3;

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

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 4;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 3;

            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = card.BaseCost = 20;

            PrerequisiteHandler.VerifyResourcesUsage(card, new List<ActionPlayResourcesUsage>
            {new ActionPlayResourcesUsage
                {
                    ResourceType = ResourceType.Titanium,UnitPlayed = 4,
                },
                }, p);

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithSpaceTagsAdnTitaniumEnoughWithTitaniumModifierCard()
        {
            Player p = ModelFactory.NewPlayer("toto", false);


            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p[ResourceType.Steel]
                .UnitCount = 2;
            p[ResourceType.Steel]
                .MoneyValueModifier = 2;

            p[ResourceType.Titanium]
                .UnitCount = 3;
            p[ResourceType.Titanium]
                .MoneyValueModifier = 4;

            Patent card = PatentFactory.NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = card.ModifiedCost = 20;


            PrerequisiteHandler.VerifyResourcesUsage(card, new List<ActionPlayResourcesUsage>
            {new ActionPlayResourcesUsage
                {
                    ResourceType = ResourceType.Titanium,UnitPlayed = 4,
                },
            }, p);


            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }
    }
}