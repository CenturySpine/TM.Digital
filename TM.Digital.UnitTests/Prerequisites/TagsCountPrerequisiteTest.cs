using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Cards;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Services;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class TagsCountPrerequisiteTest 
    {
        private TagsCountPrerequisite _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new TagsCountPrerequisite();


        }


        [TestMethod]
        public void TestBaseNotEnoughTags()
        {
            var patent = PatentFactory.NewPatent();

            patent.Prerequisites.TagsPrerequisites.Add(new TagsPrerequisite() { Tag = Tags.Energy, Value = 2 });
            var player = ModelFactory.NewPlayer("test", false);
            player.PlayedCards.Add(new Patent() { Tags = new List<Tags>() { Tags.Energy } });
            player.Corporation = CorporationsFactory.NewCorporation();

            var result = _target.CanPlayCard(patent, new Board(), player);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestBaseEnoughTagsInTheSameCard()
        {
            var patent = PatentFactory.NewPatent();

            patent.Prerequisites.TagsPrerequisites.Add(new TagsPrerequisite() { Tag = Tags.Energy, Value = 2 });
            var player = ModelFactory.NewPlayer("test", false);
            player.PlayedCards.Add(new Patent() { Tags = new List<Tags>() { Tags.Energy, Tags.Energy } });
            player.Corporation = CorporationsFactory.NewCorporation();
            var result = _target.CanPlayCard(patent, new Board(), player);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestBaseEnoughTagsWithDifferentCards()
        {
            var patent = PatentFactory.NewPatent();

            patent.Prerequisites.TagsPrerequisites.Add(new TagsPrerequisite() { Tag = Tags.Energy, Value = 2 });
            var player = ModelFactory.NewPlayer("test", false);
            player.PlayedCards.Add(new Patent() { Tags = new List<Tags>() { Tags.Energy } });
            player.PlayedCards.Add(new Patent() { Tags = new List<Tags>() { Tags.Energy } });
            player.Corporation = CorporationsFactory.NewCorporation();
            var result = _target.CanPlayCard(patent, new Board(), player);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestBaseEnoughTagsWithDifferentCardsAndCorporation()
        {
            var patent = PatentFactory.NewPatent();

            patent.Prerequisites.TagsPrerequisites.Add(new TagsPrerequisite() { Tag = Tags.Energy, Value = 3 });
            var player = ModelFactory.NewPlayer("test", false);

            player.Corporation = CorporationsFactory.NewCorporation();
            player.Corporation.Tags.Add(Tags.Energy);


            player.PlayedCards.Add(new Patent() { Tags = new List<Tags>() { Tags.Energy } });
            player.PlayedCards.Add(new Patent() { Tags = new List<Tags>() { Tags.Energy } });

            var result = _target.CanPlayCard(patent, new Board(), player);
            Assert.IsTrue(result);
        }
    }
}