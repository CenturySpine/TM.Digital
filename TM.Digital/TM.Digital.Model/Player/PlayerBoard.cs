using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Player
{
    public class PlayerBoard
    {
        public PlayerBoard()
        {
            Cards = new List<Patent>();
            
        }
        public List<ResourceHandler> Resources { get; set; } = new List<ResourceHandler>
        {
            new ResourceHandler {ResourceType = ResourceType.Money},
            new ResourceHandler {ResourceType = ResourceType.Steel},
            new ResourceHandler {ResourceType = ResourceType.Titanium},
            new ResourceHandler {ResourceType = ResourceType.Plant},
            new ResourceHandler {ResourceType = ResourceType.Energy},
            new ResourceHandler {ResourceType = ResourceType.Heat}
        };

        public List<Patent> Cards { get; set; }

    }
}