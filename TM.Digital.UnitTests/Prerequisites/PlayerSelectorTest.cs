using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TM.Digital.Cards;
using TM.Digital.Client.Screens.ActionChoice;
using TM.Digital.Client.Screens.HandSetup;
using TM.Digital.Client.Screens.Main;
using TM.Digital.Client.Services;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Game;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services;
using Action = TM.Digital.Model.Cards.Action;

namespace TM.Digital.UnitTests.Prerequisites
{
    class ApiPorxyMock : IApiProxy
    {
        public Task<Player> JoinGame(Guid selectedSessionGameSessionId, string playerName)
        {
            throw new NotImplementedException();
        }

        public Task<GameSessions> GetGameSessions()
        {
            throw new NotImplementedException();
        }

        public Task<GameSessionInformation> CreateNewGame(string playerName, int numberOfPlayers)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartGame(Guid selectedSessionGameSessionId)
        {
            throw new NotImplementedException();
        }

        public Task<Player> SendSetup(GameSetupSelection gSetup)
        {
            throw new NotImplementedException();
        }

        public Task PlaceTile(BoardPlace bp)
        {
            throw new NotImplementedException();
        }

        public Task<Board> GetBoard()
        {
            throw new NotImplementedException();
        }

        public Task Skip()
        {
            throw new NotImplementedException();
        }

        public Task Pass()
        {
            throw new NotImplementedException();
        }

        public Task PlayBoardAction(BoardAction boardAction)
        {
            throw new NotImplementedException();
        }

        public Task PlayCardAction(Action cardAction)
        {
            throw new NotImplementedException();
        }

        public Task PlayConvertResourcesAction(ResourceHandler rh)
        {
            throw new NotImplementedException();
        }

        public Task PlayCard(CardActionPlay cardAction)
        {
            throw new NotImplementedException();
        }

        public Task VerifyCardPlayability(PatentSelector patent)
        {
            throw new NotImplementedException();
        }

        public Task VerifyResourcesModifiedCostCardPlayability(PlayCardWithResources mod)
        {
            throw new NotImplementedException();
        }

        public Task SelectEffectTarget(ResourceEffectPlayerChooser choice)
        {
            throw new NotImplementedException();
        }
    }
    [TestClass]
    public class PlayerSelectorTest 
    {
        private PlayerSelector _playerSelector;
        private ApiPorxyMock _api;

        [TestInitialize]
        public void Setup()
        {
            var patent = PatentFactory.NewPatent();
            patent.BaseCost = patent.ModifiedCost = 34;
            patent.Tags.AddRange(new[] { Tags.Space, Tags.Building });
            var player = ModelFactory.NewPlayer("test", false);
            player.HandCards.Add(patent);
            player[ResourceType.Titanium].UnitCount = 3;
            player[ResourceType.Titanium].UnitCount = 3;
            player[ResourceType.Titanium].MoneyValueModifier = 3;
            player[ResourceType.Steel].MoneyValueModifier = 2;
            GameData.PlayerId = player.PlayerId;
            GameData.GameId = Guid.NewGuid();
            _api = new ApiPorxyMock();
            _playerSelector = new PlayerSelector(_api);
        }

        [TestMethod]
        public void TestPatentCostBaseNotEnough()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 20;


            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsFalse(baseResult);
        }

        [TestMethod]
        public void TestPatentCostBaseEnough()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 35;
            
            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsTrue(baseResult);
        }

        [TestMethod]
        public void TestPatentCostBaseEnoughWithModifiers()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 20;


            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[0].UnitsUsed = 3;//+9
            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[1].UnitsUsed = 3;//+6
            //total value should be 20 + 9 + 6 = 35

            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsTrue(baseResult);
        }

        [TestMethod]
        public void TestPatentCostBaseNotEnoughWithModifiers()
        {
            _playerSelector.Player[ResourceType.Money].UnitCount = 20;


            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[0].UnitsUsed = 2;//+6
            _playerSelector.PatentsSelectors[0].MineralsPatentModifiersSummary.MineralsPatentModifier[1].UnitsUsed = 3;//+6
            //total value should be 20 + 6 + 6 = 32

            var baseResult = _playerSelector.SelectCardCommand.CanExecute(_playerSelector.PatentsSelectors[0]);
            Assert.IsFalse(baseResult);
        }
    }
}