using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Player;
using TM.Digital.Model.Prerequisite;
using TM.Digital.Model.Tile;
using TM.Digital.Services;

namespace TM.Digital.UnitTests.Prerequisites
{
    [TestClass]
    public class TilePrerequisiteTest
    {
        private Player _player;
        private Board _board;
        private Patent _p;
        private TilePrerequisiteStrategy _target;

        [TestInitialize]
        public void Setup()
        {
            _player = ModelFactory.NewPlayer("test", false);
            _board = BoardGenerator.Instance.BoardShell();
            _p = new Patent
            {
                Prerequisites = new Model.Cards.Prerequisites
                {
                    TilePrerequisites = new List<TilePrerequisite>
                    {
                        new TilePrerequisite
                        {
                            Value = 2,
                            PrerequisiteKind = PrerequisiteKind.Board,
                            LocationConstraint = EffectModifierLocationConstraint.Anywhere,
                            TileType = TileType.City
                        }
                    }
                }
            };

            _target = new TilePrerequisiteStrategy();
        }

        [TestMethod]
        public void TestBase()
        {
            _board.BoardLines[0].BoardPlaces[1].PlayedTile = new Tile { Type = TileType.City, Owner = _player.PlayerId };

            Assert.IsFalse(_target.CanPlayCard(_p, _board, _player));
        }
        [TestMethod]
        public void TestBase02()
        {
            _board.BoardLines[0].BoardPlaces[1].PlayedTile = new Tile { Type = TileType.City, Owner = _player.PlayerId };
            _board.BoardLines[3].BoardPlaces[2].PlayedTile = new Tile { Type = TileType.City, Owner = Guid.NewGuid() };

            Assert.IsTrue(_target.CanPlayCard(_p, _board, _player));
        }

        [TestMethod]
        public void TestBase03()
        {
            _board.BoardLines[0].BoardPlaces[1].PlayedTile = new Tile { Type = TileType.City, Owner = _player.PlayerId };
            _board.IsolatedPlaces[1].PlayedTile = new Tile { Type = TileType.City, Owner = Guid.NewGuid() };

            Assert.IsTrue(_target.CanPlayCard(_p, _board, _player));
        }

        [TestMethod]
        public void TestBase04()
        {
            _board.BoardLines[0].BoardPlaces[1].PlayedTile = new Tile { Type = TileType.Forest, Owner = Guid.NewGuid() };

            _p.Prerequisites.TilePrerequisites = new List<TilePrerequisite>()
            {
                new TilePrerequisite()
                {
                    Value = 1,
                    PrerequisiteKind = PrerequisiteKind.Self,
                    TileType = TileType.Forest
                }
            };

            Assert.IsFalse(_target.CanPlayCard(_p, _board, _player));
        }

        [TestMethod]
        public void TestBase05()
        {
            _board.BoardLines[0].BoardPlaces[1].PlayedTile = new Tile { Type = TileType.Forest, Owner = _player.PlayerId };

            _p.Prerequisites.TilePrerequisites = new List<TilePrerequisite>()
            {
                new TilePrerequisite()
                {
                    Value = 1,
                    PrerequisiteKind = PrerequisiteKind.Self,
                    TileType = TileType.Forest
                }
            };

            Assert.IsTrue(_target.CanPlayCard(_p, _board, _player));
        }
    }
}