using System.Linq;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

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
            Player = new Model.Player.Player();
            PlayerResource(ResourceType.Energy).Production = 2;
            PlayerResource(ResourceType.Energy).UnitCount = 5;
            PlayerResource(ResourceType.Money).Production = 1;
            PlayerResource(ResourceType.Money).UnitCount = 40;
        }
    }
}