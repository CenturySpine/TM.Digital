using System.Linq;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;
using TM.Digital.Services;

namespace TM.Digital.UnitTests
{
    public class EffectHandlerTestsBase
    {
        protected ResourceHandler PlayerResource(ResourceType type)
        {
            return Player.Resources.First(r => r.ResourceType == type);
        }
        protected Player Player;
        protected void SetupPlayer()
        {
            Player = ModelFactory.NewPlayer("test", false);
            PlayerResource(ResourceType.Energy).Production = 2;
            PlayerResource(ResourceType.Energy).UnitCount = 5;
            PlayerResource(ResourceType.Money).Production = 1;
            PlayerResource(ResourceType.Money).UnitCount = 40;
        }
    }
}