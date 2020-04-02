using System.Collections.Generic;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Prerequisite;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;
using GlobalPrerequisite = TM.Digital.Model.Cards.GlobalPrerequisite;
using TagsPrerequisite = TM.Digital.Model.Cards.TagsPrerequisite;

namespace TM.Digital.Cards
{
    public class PatentFactory
    {
        public static Patent SolarWindEnergy()
        {
            return new Patent
            {
                Name = "Solar wind energy",
                BaseCost = 11,
                Tags = new List<Tags> { Tags.Science, Tags.Space, Tags.Energy },
                ResourcesEffects = new List<ResourceEffect>
                {
                    new ResourceEffect
                    {
                        Amount = 2,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Titanium
                    },
                    new ResourceEffect {Amount = 1,ResourceKind = ResourceKind.Production,ResourceType = ResourceType.Energy}
                }
            };
        }

        public static Patent BubbleCity()
        {
            return new Patent
            {
                Name = "Bubble city",
                BaseCost = 16,
                Tags = new List<Tags> { Tags.Building, Tags.City },
                ResourcesEffects = new List<ResourceEffect>
                {
                    new ResourceEffect
                    {
                        Amount = -1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Energy
                    },
                    new ResourceEffect {Amount = 3,ResourceKind = ResourceKind.Production,ResourceType = ResourceType.Money}
                },
                TileEffects = new List<TileEffect>
                {
                    new TileEffect
                    {
                        Number = 1,Type = TileType.City
                    }
                },
                GlobalPrerequisites = new List<GlobalPrerequisite>() { new GlobalPrerequisite()
                {
                    IsMax = true,Parameter = GlobalParameterType.Oxygen,PrerequisiteKind = PrerequisiteKind.Board,Value = 9
                } }
            };
        }

        public static Patent GiantAsteroid()
        {
            return new Patent
            {
                Name = "Giant asteroid",
                BaseCost = 27,
                Tags = new List<Tags> { Tags.Space, Tags.Event },
                ResourcesEffects = new List<ResourceEffect>
                {
                    new ResourceEffect
                    {
                        Amount = 4,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Titanium
                    },
                    new ResourceEffect {Amount = -3,ResourceKind = ResourceKind.Unit,ResourceType = ResourceType.Plant, EffectDestination = EffectDestination.OtherPlayer}
                },
                GlobalParameterEffects = new List<GlobalParameterLevelEffect>()
{
    new GlobalParameterLevelEffect()
    {
        GlobalParameterType = GlobalParameterType.Temperature,Level = 2
    }
}
            };
        }

        public static Patent FusionEnergy()
        {
            return new Patent
            {
                Name = "Energy Fusion",
                BaseCost = 14,
                Tags = new List<Tags> { Tags.Energy, Tags.City },
                ResourcesEffects = new List<ResourceEffect>
                {
                    new ResourceEffect
                    {
                        Amount = 3,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Energy
                    },

                },

                TagsPrerequisites = new List<TagsPrerequisite>() { new TagsPrerequisite() { PrerequisiteKind = PrerequisiteKind.Self, Value = 2, Tag = Tags.Energy } }
            };
        }

        public static Patent ThreeDimensionalHomePrinting()
        {
            return new Patent
            {
                Name = "3D home printing",
                BaseCost = 10,
                Tags = new List<Tags> { Tags.Building },
                ResourcesEffects = new List<ResourceEffect>
                {
                    new ResourceEffect
                    {
                        Amount = 1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Steel
                    },

                },

                CardVictoryPoints = new StandardVictoryPoint() { Points = 1 }
            };
        }
        public static Patent ToundraAgriculture()
        {
            return new Patent
            {
                Name = "Toundra agriculture",
                BaseCost = 16,
                Tags = new List<Tags> { Tags.Plant },
                ResourcesEffects = new List<ResourceEffect>
                {
                    new ResourceEffect
                    {
                        Amount = 1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Plant
                    },
                    new ResourceEffect
                    {
                        Amount = 1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Money
                    },
                    new ResourceEffect
                    {
                        Amount = 1,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Plant
                    },
                },

                CardVictoryPoints = new StandardVictoryPoint() { Points = 2 },
                GlobalPrerequisites = new List<GlobalPrerequisite>() { new GlobalPrerequisite()
                {
                    Parameter = GlobalParameterType.Temperature,PrerequisiteKind = PrerequisiteKind.Board,Value = -6
                } }

            };
        }

        public static Patent IdleGazLiberation()
        {
            return new Patent
            {
                Name = "Idle gaz liberation",
                BaseCost = 14,
                Tags = new List<Tags> { Tags.Event },
                GlobalParameterEffects = new List<GlobalParameterLevelEffect>() { new GlobalParameterLevelEffect() { Level = 2, GlobalParameterType = GlobalParameterType.Terraformation } }

            };
        }
    }
}