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
                return new Corporation
                {
                    TagEffects = new TagsEffects(),
                    Tags = new TagsList(),
                    BoardEffects = new List<BoardLevelEffect>(),
                    ResourcesEffects = new List<ResourceEffect>(),
                    CardVictoryPoints = new StandardVictoryPoint(),
                    CardResourcesVictoryPoints = new ResourcesVictoryPoints(),
                    TileEffects = new List<TileEffect>()

                };
            }
      
        
        //public static Corporation CheungShingMars()
        //{
        //    var c = new Corporation
        //    {
        //        Name = "Cheung Shing Mars",
        //        //StartingMoney = 44,
                
        //        Tags = new TagsList()
        //    };
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 44, ResourceType = ResourceType.Money });
        //    c.TagEffects.Add(new TagEffect { AffectedTags = new TagsList{ Tags.Building }, ResourceEffects = new List<ResourceEffect>
        //    {
        //        new ResourceEffect
        //        {
        //            ResourceType = ResourceType.Money,ResourceKind = ResourceKind.Unit,Amount = -2
        //        }
        //    }, TagEffectType = TagEffectType.CostAlteration});

        //    c.Tags.Add(Tags.Building);
        //    return c;
        //}

        //public static Corporation Teractor()
        //{
        //    var c = new Corporation
        //    {
        //        Name = "Terractor",
        //        //StartingMoney = 60,
                
        //        Tags = new TagsList()
        //    };
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 60, ResourceType = ResourceType.Money });
        //    c.TagEffects.Add(new TagEffect { AffectedTags = new TagsList { Tags.Earth },
        //        ResourceEffects = new List<ResourceEffect>
        //        {
        //            new ResourceEffect
        //            {
        //                ResourceType = ResourceType.Money,ResourceKind = ResourceKind.Unit,Amount = -3
        //            }
        //        }, TagEffectType = TagEffectType.CostAlteration });

        //    c.Tags.Add(Tags.Earth);
        //    return c;
        //}

        //public static Corporation PhobLog()
        //{
        //    var c = new Corporation
        //    {
        //        Name = "Phoblog",
        //        //StartingMoney = 23,

        //        Tags = new TagsList()
        //    };
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 23, ResourceType = ResourceType.Money });
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 10, ResourceType = ResourceType.Titanium });

        //    c.MineralModifiers = new MineralModifiers {TitaniumModifier = new MineralModifier{ResourceType = ResourceType.Titanium, Value = 1}, SteelModifier = new MineralModifier { ResourceType = ResourceType.Steel, Value = 0 } };
        //    c.Tags.Add(Tags.Space);
        //    return c;
        //}


        //public static Corporation InterPlanetaryCinematics()
        //{
        //    var c = new Corporation
        //    {
        //        Name = "Interplanetary Cinematics",
        //        //StartingMoney = 30,
                
        //        Tags = new TagsList()
        //    };
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 30, ResourceType = ResourceType.Money });
        //    c.TagEffects.Add(new TagEffect { AffectedTags = new TagsList { Tags.Event },
        //        ResourceEffects = new List<ResourceEffect>
        //        {
        //            new ResourceEffect
        //            {
        //                ResourceType = ResourceType.Money,ResourceKind = ResourceKind.Unit,Amount = 2
        //            }
        //        }, TagEffectType = TagEffectType.PlayReward });
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 20, ResourceType = ResourceType.Steel,ResourceKind = ResourceKind.Unit});

        //    c.Tags.Add(Tags.Building);
        //    return c;
        //}

        //public static Corporation Arklight()
        //{
        //    var c = new Corporation
        //    {
        //        Name = "Arklight",
        //        //StartingMoney = 45,
        //        CardResourcesVictoryPoints = new ResourcesVictoryPoints
        //        {
        //            ResourceType = ResourceType.Animal,
        //            VictoryPointRatio = 2
        //        },
                
        //        Tags = new TagsList()
        //    }; 
        //    c.ResourcesEffects.Add(new ResourceEffect { Amount = 45, ResourceType = ResourceType.Money });

        //    c.ResourcesEffects.Add(new ResourceEffect { ResourceType = ResourceType.Money, Amount = 2, ResourceKind = ResourceKind.Production});
        //    c.TagEffects.Add(new TagEffect { AffectedTags = new TagsList { Tags.Animal, Tags.Plant },
        //        ResourceEffects = new List<ResourceEffect>
        //        {
        //            new ResourceEffect
        //            {
        //                ResourceType = ResourceType.Animal,ResourceKind = ResourceKind.Unit,Amount = 1
        //            }
        //        }, TagEffectType = TagEffectType.PlayReward });
        //    //c.TagEffects.Add(new TagEffect { AffectedTags = new TagsList { Tags.Plant }, ResourceKind = ResourceKind.Unit, ResourceType = ResourceType.Animal, EffectValue = 1, TagEffectType = TagEffectType.PlayReward });

        //    c.Tags.Add(Tags.Animal);
        //    return c;
        //}
    }
}