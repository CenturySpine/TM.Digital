using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Cards;
using TM.Digital.Model.Board;
using TM.Digital.Model.Prerequisite;
using TM.Digital.Services;
using GlobalPrerequisite = TM.Digital.Model.Cards.GlobalPrerequisite;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class GlobalParametersCheckTests 
    {
        private GlobalCheckPrerequisite _target;
        private Board board;

        [TestInitialize]
        public void Setup()
        {
            _target = new GlobalCheckPrerequisite();
            board = BoardGenerator.Instance.Original();

        }

        private void FillParameters(BoardParameter first, int i)
        {
            foreach (var globalParameterLevel in first.Levels)
            {
                if (globalParameterLevel.Level <= i)
                {
                    globalParameterLevel.IsFilled = true;
                }
            }
        }
        [TestMethod]
        public void TestTemperatureMinNotReached()
        {
            var patent = PatentFactory.NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite
            {
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            FillParameters(board.Parameters.First(p => p.Type == BoardLevelType.Temperature), -8);



            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsFalse(result);
        }



        [TestMethod]
        public void TestTemperatureMaxOverReached()
        {
            var patent = PatentFactory.NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite
            {
                IsMax = true,
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });
            FillParameters(board.Parameters.First(p => p.Type == BoardLevelType.Temperature), -4);



            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestTemperatureMinOk()
        {
            var patent = PatentFactory.NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite
            {
              
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            FillParameters(board.Parameters.First(p => p.Type == BoardLevelType.Temperature), -4);


            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestTemperatureMaxOk()
        {
            var patent = PatentFactory.NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite
            {
                IsMax = true,
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            FillParameters(board.Parameters.First(p => p.Type == BoardLevelType.Temperature), -8);


            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsTrue(result);
        }

    }
}
