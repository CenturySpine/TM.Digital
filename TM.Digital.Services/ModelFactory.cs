using System;
using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Player;
using TM.Digital.Model.Resources;

namespace TM.Digital.Services
{
    public static class ModelFactory
    {

        public static Player NewPlayer(string name, bool test)
        {
            return new Player
            {
                IsReady = false,
                TotalActions = 2,
                RemainingActions = 0,
                PlayerId = Guid.NewGuid(),
                Name = name,
                Resources = new List<ResourceHandler>
                {
                    new ResourceHandler {ResourceType = ResourceType.Money,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Steel,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Titanium,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Plant,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Energy,UnitCount = test?20:0,Production = test?5:0},
                    new ResourceHandler {ResourceType = ResourceType.Heat,UnitCount = test?20:0,Production = test?5:0}
                },
                HandCards = new List<Patent>(),
                PlayedCards = new List<Patent>(),
                Corporation = new Corporation(),
                TerraformationLevel = 20,

            };
        }
    }
}