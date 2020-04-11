using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Board;
using TM.Digital.Model.Prerequisite;
using TM.Digital.Services;
using GlobalPrerequisite = TM.Digital.Model.Cards.GlobalPrerequisite;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class GlobalParametersCheckTests : TestPatentCheckClassAbase
    {
        private GlobalCheckPrerequisite _target;
        private Board board;

        [TestInitialize]
        public void Setup()
        {
            _target = new GlobalCheckPrerequisite();
            board = BoardGenerator.Instance.Original();

        }


        [TestMethod]
        public void TestTemperatureMinNotReached()
        {
            var patent = NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite()
            {
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            board.Parameters.First(p => p.Type == BoardLevelType.Temperature).GlobalParameterLevel = new GlobalParameterLevel()
            {
                Level = -8
            };


            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestTemperatureMaxOverReached()
        {
            var patent = NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite()
            {
                IsMax = true,
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            board.Parameters.First(p => p.Type == BoardLevelType.Temperature).GlobalParameterLevel = new GlobalParameterLevel()
            {
                Level = -4
            };


            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestTemperatureMinOk()
        {
            var patent = NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite()
            {
              
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            board.Parameters.First(p => p.Type == BoardLevelType.Temperature).GlobalParameterLevel = new GlobalParameterLevel()
            {
                Level = -4
            };


            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestTemperatureMaxOk()
        {
            var patent = NewPatent();
            patent.Prerequisites.GlobalPrerequisites.Add(new GlobalPrerequisite()
            {
                IsMax = true,
                Value = -6,
                Parameter = BoardLevelType.Temperature,
                PrerequisiteKind = PrerequisiteKind.Board
            });

            board.Parameters.First(p => p.Type == BoardLevelType.Temperature).GlobalParameterLevel = new GlobalParameterLevel()
            {
                Level = -8
            };


            var result = _target.CanPlayCard(patent, board, ModelFactory.NewPlayer("", false));

            Assert.IsTrue(result);
        }

    }
}
