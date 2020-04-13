using System;
using System.Collections.Generic;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;
using TM.Digital.Model.Tile;

namespace TM.Digital.Model.Cards
{
    public class Card
    {
        protected bool Equals(Card other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Card)obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public string OfficialNumberTag { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public List<Tags> Tags { get; set; }

        public CardType CardType { get; set; }

        public ResourceType ResourceType { get; set; }

        public int ResourcesCount { get; set; }

        
        public StandardVictoryPoint CardVictoryPoints { get; set; }
        
        public ResourcesVictoryPoints CardResourcesVictoryPoints { get; set; }

        public List<ResourceEffect> ResourcesEffects { get; set; } = new List<ResourceEffect>();

        public int TitaniumValueModifier { get; set; }
        public int SteelValueModifier { get; set; }
        public List<BoardLevelEffect> BoardEffects { get; set; } = new List<BoardLevelEffect>();

        public List<TagEffect> TagEffects { get; set; } = new List<TagEffect>();

        public List<TileEffect> TileEffects { get; set; } = new List<TileEffect>();

        public List<Action> Actions { get; set; }
        public int PlantsConversionRate { get; set; }
        public int HeatConversionRate { get; set; }
    }

    public class Action
    {
        

        public ActionFrom ActionFrom { get; set; }

        public ActionTo ActionTo { get; set; }

        public ActionModifier ActionModifier { get; set; }

    }

    public class ActionModifier
    {
        public ActionTarget ActionTarget { get; set; }
    }

    public class ActionFrom
    {
        public ActionTarget ActionTarget { get; set; }

        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }
        public ResourceKind ResourceKind { get; set; }
    }

    public class ActionTo
    {
        public ActionTarget ActionTarget { get; set; }

        public ResourceType ResourceType { get; set; }
        public int Amount { get; set; }
        public ResourceKind ResourceKind { get; set; }
    }

    public enum ActionTarget
    {
        Self,
        CurrentCard,
        AnyOtherCard,
        AnyPlayer
    }

}