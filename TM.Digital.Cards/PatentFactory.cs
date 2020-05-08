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
        public static Patent NewPatent()
        {
            return new Patent
            {
                BoardEffects = new List<BoardLevelEffect>(),
                CardResourcesVictoryPoints = new ResourcesVictoryPoints(),
                CardVictoryPoints = new StandardVictoryPoint(),

                Prerequisites = new Model.Cards.Prerequisites
                {
                    TagsPrerequisites = new List<TagsPrerequisite>(),
                    GlobalPrerequisites = new List<GlobalPrerequisite>()
                },
                TagEffects = new TagsEffects(),
                Tags = new TagsList(),
                TileEffects = new List<TileEffect>()
            };
        }
//        public static Patent AdvancedAlliages()
//        {
//            return new Patent
//            {
//                Name = "Advanced alliages",
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                BaseCost = 9,
//                Tags = new TagsList { Tags.Science },
//                MineralModifiers = new MineralModifiers
//                {
//                    SteelModifier = new MineralModifier { ResourceType = ResourceType.Steel,Value = 1}
//                    ,
//                    TitaniumModifier = new MineralModifier { ResourceType = ResourceType.Titanium, Value = 1 }
//                }
                
//            };
//        }

//        public static Patent BubbleCity()
//        {
//            return new Patent
//            {
//                Name = "Bubble city",
//                BaseCost = 16,
//                Tags = new TagsList { Tags.Building, Tags.City },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = -1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Energy
//                    },
//                    new ResourceEffect {Amount = 3,ResourceKind = ResourceKind.Production,ResourceType = ResourceType.Money}
//                },
//                TileEffects = new List<TileEffect>
//                {
//                    new TileEffect
//                    {
//                        Number = 1,Type = TileType.City,Constrains = TilePlacementCosntrains.StandardCity
//                    }
//                },
//                Prerequisites = new Prerequisites
//                {
//                    TagsPrerequisites = new List<TagsPrerequisite>(),
//                    GlobalPrerequisites = new List<GlobalPrerequisite>
//                    { new GlobalPrerequisite
//                        {
//                            IsMax = true,Parameter = BoardLevelType.Oxygen,PrerequisiteKind = PrerequisiteKind.Board,Value = 9
//                        },

//                }
//                },

//            };
//        }

//        public static Patent FusionEnergy()
//        {
//            return new Patent
//            {
//                Name = "Energy Fusion",
//                BaseCost = 14,
//                Tags = new TagsList { Tags.Energy, Tags.City },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = 3,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Energy
//                    },
//                },
//                Prerequisites = new Prerequisites
//                {
//                    GlobalPrerequisites = new List<GlobalPrerequisite>(),
//                    TagsPrerequisites = new List<TagsPrerequisite> { new TagsPrerequisite { PrerequisiteKind = PrerequisiteKind.Self, Value = 2, Tag = Tags.Energy } }
//                }
//            };
//        }

//        public static Patent GiantAsteroid()
//        {
//            return new Patent
//            {
//                Name = "Giant asteroid",
//                BaseCost = 27,
//                Tags = new TagsList { Tags.Space, Tags.Event },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = 4,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Titanium
//                    },
//                    new ResourceEffect {Amount = -4,ResourceKind = ResourceKind.Unit,ResourceType = ResourceType.Plant, EffectDestination = ActionTarget.AnyPlayer}
//                },
//                BoardEffects = new List<BoardLevelEffect>
//                {
//    new BoardLevelEffect
//    {
//        BoardLevelType = BoardLevelType.Temperature,Level = 2
//    }
//}
//            };
//        }

//        public static Patent IdleGazLiberation()
//        {
//            return new Patent
//            {
//                Name = "Idle gaz liberation",
//                BaseCost = 14,
//                Tags = new TagsList { Tags.Event },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                BoardEffects = new List<BoardLevelEffect> { new BoardLevelEffect { Level = 2, BoardLevelType = BoardLevelType.Terraformation } }
//            };
//        }

//        public static Patent SolarWindEnergy()
//        {
//            return new Patent
//            {
//                Name = "Solar wind energy",
//                BaseCost = 11,
//                Tags = new TagsList { Tags.Science, Tags.Space, Tags.Energy },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = 2,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Titanium
//                    },
//                    new ResourceEffect {Amount = 1,ResourceKind = ResourceKind.Production,ResourceType = ResourceType.Energy}
//                }
//            };
//        }

//        public static Patent ThreeDimensionalHomePrinting()
//        {
//            return new Patent
//            {
//                Name = "3D home printing",
//                BaseCost = 10,
//                Tags = new TagsList { Tags.Building },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = 1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Steel
//                    },
//                },

//                CardVictoryPoints = new StandardVictoryPoint { Points = 1 }
//            };
//        }

//        public static Patent ProtectedValley()
//        {
//            return new Patent
//            {
//                Name = "Protected valley",
//                BaseCost = 23,
//                Tags = new TagsList { Tags.Building, Tags.Plant },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = 2,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Money
//                    },
//                },

//                TileEffects = new List<TileEffect>
//                { new TileEffect
//                { Type = TileType.Forest,Number = 1,
//                    Constrains = TilePlacementCosntrains.ReservedForOcean} }
//            };
//        }
//        public static Patent Comet()
//        {
//            return new Patent
//            {
//                Name = "Comet",
//                BaseCost = 21,
//                Tags = new TagsList { Tags.Space, Tags.Event },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = -3,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Plant,EffectDestination = ActionTarget.AnyPlayer
//                    },
//                },

//                TileEffects = new List<TileEffect> { new TileEffect { Type = TileType.Ocean, Number = 1, Constrains = TilePlacementCosntrains.ReservedForOcean } },
//                BoardEffects = new List<BoardLevelEffect> { new BoardLevelEffect { Level = 1, BoardLevelType = BoardLevelType.Temperature } }
//            };
//        }

//        public static Patent GiantIceAsteroid()
//        {
//            return new Patent
//            {
//                Name = "Giant ice asteroid",
//                BaseCost = 36,
//                Tags = new TagsList { Tags.Space, Tags.Event },
//                Prerequisites = new Prerequisites { GlobalPrerequisites = new List<GlobalPrerequisite>(), TagsPrerequisites = new List<TagsPrerequisite>() },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = -6,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Plant,EffectDestination = ActionTarget.AnyPlayer
//                    },
//                },

//                TileEffects = new List<TileEffect>
//                {
//                    new TileEffect { Type = TileType.Ocean, Number = 1, Constrains = TilePlacementCosntrains.ReservedForOcean },
//                    new TileEffect { Type = TileType.Ocean, Number = 1, Constrains = TilePlacementCosntrains.ReservedForOcean }
//                },
//                BoardEffects = new List<BoardLevelEffect>
//                {
//                    new BoardLevelEffect { Level = 1, BoardLevelType = BoardLevelType.Temperature },
//                    new BoardLevelEffect { Level = 1, BoardLevelType = BoardLevelType.Temperature }
//                }
//            };
//        }

//        public static Patent ToundraAgriculture()
//        {
//            return new Patent
//            {
//                Name = "Toundra agriculture",
//                BaseCost = 16,
//                Tags = new TagsList { Tags.Plant },
//                ResourcesEffects = new List<ResourceEffect>
//                {
//                    new ResourceEffect
//                    {
//                        Amount = 1,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Plant
//                    },
//                    new ResourceEffect
//                    {
//                        Amount = 2,ResourceKind =ResourceKind.Production,ResourceType = ResourceType.Money
//                    },
//                    new ResourceEffect
//                    {
//                        Amount = 1,ResourceKind =ResourceKind.Unit,ResourceType = ResourceType.Plant
//                    },
//                },

//                CardVictoryPoints = new StandardVictoryPoint { Points = 2 },
//                Prerequisites = new Prerequisites
//                {
//                    TagsPrerequisites = new List<TagsPrerequisite>(),
//                    GlobalPrerequisites = new List<GlobalPrerequisite>
//                { new GlobalPrerequisite
//                {
//                    Parameter = BoardLevelType.Temperature,PrerequisiteKind = PrerequisiteKind.Board,Value = -6
//                } }
//                }
//            };
//        }
    }
}