using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using TM.Digital.Services;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class PatentCostTests: TestPatentCheckClassAbase
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

            Patent card = NewPatent();
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

            Patent card = NewPatent();
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


            Patent card = NewPatent();
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


            Patent card = NewPatent();
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


            Patent card = NewPatent();
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


            Patent card = NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = 20;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }

        [TestMethod]
        public void TestCostWithSpaceTagsAdnTitaniumEnoughWithTitaniumModifierCard()
        {
            Player p = ModelFactory.NewPlayer("toto", false);
            var playedPatent = NewPatent();
            playedPatent.TitaniumValueModifier = 1;
            p.PlayedCards.Add(playedPatent);

            p.Resources.First(r => r.ResourceType == ResourceType.Money)
                .UnitCount = 10;

            p.Resources.First(r => r.ResourceType == ResourceType.Steel)
                .UnitCount = 2;

            p.Resources.First(r => r.ResourceType == ResourceType.Titanium)
                .UnitCount = 3;


            Patent card = NewPatent();
            card.Tags.Add(Tags.Space);
            card.ModifiedCost = 20;

            var canPlay = _target.CanPlayCard(card, new Board(), p);

            Assert.IsTrue(canPlay);
        }


    }

    public class TestPatentCheckClassAbase
    {
        public static Patent NewPatent()
        {
            return new Patent()
            {
                BoardEffects = new List<BoardLevelEffect>(),
                CardResourcesVictoryPoints = new ResourcesVictoryPoints(),
                Prerequisites = new Model.Cards.Prerequisites()
                {
                    TagsPrerequisites = new List<TagsPrerequisite>(),
                    GlobalPrerequisites = new List<GlobalPrerequisite>()
                },
                TagEffects = new List<TagEffect>(),
                Tags = new List<Tags>(),
                TileEffects = new List<TileEffect>()
            };
        }
    }


}
