using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Cards
{
    public class CheungShingMars : Corporation
    {
        public CheungShingMars()
        {
            StartingMoney = 44;
            Effects.Add(new ResourceProductionEffect { Amount = 3, ResourceType = ResourceType.Money });
            Effects.Add(new TagCardCostEffect { AffectedTag = Model.Cards.Tags.Building, CostEffect = -2 });

            Tags.Add(Model.Cards.Tags.Building);
        }
    }

    public class Teractor : Corporation
    {
        public Teractor()
        {
            StartingMoney = 60;

            Effects.Add(new TagCardCostEffect { AffectedTag = Model.Cards.Tags.Earth, CostEffect = -3 });

            Tags.Add(Model.Cards.Tags.Earth);
        }
    }

    public class InterPlanetaryCinematics : Corporation
    {
        public InterPlanetaryCinematics()
        {
            StartingMoney = 30;

            Effects.Add(new TagPlayRewardEffect { AffectedTag = Model.Cards.Tags.Event, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Money, RewardAmount = 2 });
            Effects.Add(new ResourceUnitEffect { Amount = 20, ResourceType = ResourceType.Steel });

            Tags.Add(Model.Cards.Tags.Building);
        }
    }

    public class Arklight : Corporation
    {
        public Arklight()
        {
            StartingMoney = 45;

            ResourceType = ResourceType.Animal;

            CardVictoryPoints = new ResourcesVictoryPoints { ResourceType = ResourceType.Animal, VictoryPointRatio = 2 };

            Effects.Add(new ResourceProductionEffect { ResourceType = ResourceType.Money, Amount = 2 });
            Effects.Add(new TagPlayRewardEffect { AffectedTag = Model.Cards.Tags.Animal, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Animal, RewardAmount = 1 });
            Effects.Add(new TagPlayRewardEffect { AffectedTag = Model.Cards.Tags.Plant, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Animal, RewardAmount = 1 });

            Tags.Add(Model.Cards.Tags.Animal);
        }
    }
}