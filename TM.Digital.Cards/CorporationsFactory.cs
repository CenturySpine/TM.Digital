using System.Collections.Generic;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Cards
{
    public static class CorporationsFactory
    {


            public static Corporation NewCorporation()
            {
                return new Corporation()
                {
                    TagEffects = new List<TagEffect>(),
                    Tags = new List<Tags>(),
                    BoardEffects = new List<BoardLevelEffect>(),
                    ResourcesEffects = new List<ResourceEffect>(),
                    CardVictoryPoints = new StandardVictoryPoint(),
                    CardResourcesVictoryPoints = new ResourcesVictoryPoints(),
                    TileEffects = new List<TileEffect>()

                };
            }
      
        
        public static Corporation CheungShingMars()
        {
            var c = new Corporation
            {
                Name = "Cheung Shing Mars",
                //StartingMoney = 44,
                
                Tags = new List<Tags>()
            };
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 44, ResourceType = ResourceType.Money });
            c.TagEffects.Add(new TagEffect { AffectedTag = Tags.Building, EffectValue = -2 , TagEffectType = TagEffectType.CostAlteration});

            c.Tags.Add(Tags.Building);
            return c;
        }

        public static Corporation Teractor()
        {
            var c = new Corporation
            {
                Name = "Terractor",
                //StartingMoney = 60,
                
                Tags = new List<Tags>()
            };
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 60, ResourceType = ResourceType.Money });
            c.TagEffects.Add(new TagEffect { AffectedTag = Tags.Earth, EffectValue = -3 , TagEffectType = TagEffectType.CostAlteration });

            c.Tags.Add(Tags.Earth);
            return c;
        }

        public static Corporation PhobLog()
        {
            var c = new Corporation
            {
                Name = "Phoblog",
                //StartingMoney = 23,

                Tags = new List<Tags>()
            };
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 23, ResourceType = ResourceType.Money });
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 10, ResourceType = ResourceType.Titanium });

            c.TitaniumValueModifier = 1;
            c.Tags.Add(Tags.Space);
            return c;
        }


        public static Corporation InterPlanetaryCinematics()
        {
            var c = new Corporation
            {
                Name = "Interplanetary Cinematics",
                //StartingMoney = 30,
                
                Tags = new List<Tags>()
            };
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 30, ResourceType = ResourceType.Money });
            c.TagEffects.Add(new TagEffect { AffectedTag = Tags.Event, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Money, EffectValue = 2 , TagEffectType = TagEffectType.PlayReward });
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 20, ResourceType = ResourceType.Steel,ResourceKind = ResourceKind.Unit});

            c.Tags.Add(Tags.Building);
            return c;
        }

        public static Corporation Arklight()
        {
            var c = new Corporation
            {
                Name = "Arklight",
                //StartingMoney = 45,
                CardResourcesVictoryPoints = new ResourcesVictoryPoints
                {
                    ResourceType = ResourceType.Animal,
                    VictoryPointRatio = 2
                },
                
                Tags = new List<Tags>()
            }; 
            c.ResourcesEffects.Add(new ResourceEffect { Amount = 45, ResourceType = ResourceType.Money });

            c.ResourcesEffects.Add(new ResourceEffect { ResourceType = ResourceType.Money, Amount = 2, ResourceKind = ResourceKind.Production});
            c.TagEffects.Add(new TagEffect { AffectedTag = Tags.Animal, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Animal, EffectValue = 1, TagEffectType = TagEffectType.PlayReward });
            c.TagEffects.Add(new TagEffect { AffectedTag = Tags.Plant, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Animal, EffectValue = 1, TagEffectType = TagEffectType.PlayReward });

            c.Tags.Add(Tags.Animal);
            return c;
        }
    }
}