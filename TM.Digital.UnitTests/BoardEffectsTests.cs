using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TM.Digital.Model.Board;
using TM.Digital.Services;

namespace TM.Digital.UnitTests
{
    [TestClass]
    public class BoardEffectsTests : EffectHandlerTestsBase
    {
        [TestMethod]
        public void TestMaxParameter()

        {
            var board = BoardGenerator.Instance.Original();

            for (var index = 0; index < board.Parameters[0].Levels.Count; index++)
            {
                var globalParameterLevel = board.Parameters[0].Levels[index];
                if (index < board.Parameters[0].Levels.Count - 1)
                    globalParameterLevel.IsFilled = true;
            }

            SetupPlayer();
            Player.TerraformationLevel = 10;

            BoardEffectHandler
                .IncreaseParameterLevel(board, BoardLevelType.Temperature, Player, 2, new CardDrawer())
                .GetAwaiter()
                .GetResult();

            Assert.AreEqual(11, Player.TerraformationLevel);
            Assert.IsTrue(board.Parameters[0].Levels.All(r=>r.IsFilled));
        }
    }
}
