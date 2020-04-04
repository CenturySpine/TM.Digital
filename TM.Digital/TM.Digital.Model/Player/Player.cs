using System;
using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Player
{
    public class Player
    {
        public Player()
        {
            TerraformationLevel = 20;
            
            HandCards = new List<Patent>();
            PlayedCards = new List<Patent>();

        }

        public Corporation Corporation { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int TerraformationLevel { get; set; }
        public List<ResourceHandler> Resources { get; set; } = new List<ResourceHandler>
        {
            new ResourceHandler {ResourceType = ResourceType.Money},
            new ResourceHandler {ResourceType = ResourceType.Steel},
            new ResourceHandler {ResourceType = ResourceType.Titanium},
            new ResourceHandler {ResourceType = ResourceType.Plant},
            new ResourceHandler {ResourceType = ResourceType.Energy},
            new ResourceHandler {ResourceType = ResourceType.Heat}
        };

        public List<Patent> HandCards { get; set; }
        public List<Patent> PlayedCards { get; set; }
    }
}