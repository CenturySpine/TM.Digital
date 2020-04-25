using System;
using System.Collections.Generic;
using System.Linq;
using TM.Digital.Model.Board;
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

        public Card()
        {

        }

        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string OfficialNumberTag { get; set; }

        public CardType CardType { get; set; }

        public TagsList Tags { get; set; }
        public List<ResourceEffect> ResourcesEffects { get; set; } = new List<ResourceEffect>();
     
        public List<ResourceEffect> ResourceEffectAlternatives { get; set; }
        public List<BoardLevelEffect> BoardEffects { get; set; } = new List<BoardLevelEffect>();
        public TagsEffects TagEffects { get; set; } = new TagsEffects();
        public List<TileEffect> TileEffects { get; set; } = new List<TileEffect>();
        public TilePassiveEffects TilePassiveEffects { get; set; } = new TilePassiveEffects();
        public List<Action> Actions { get; set; } = new List<Action>();
        public ResourceType ResourceType { get; set; }

        public int ResourcesCount { get; set; }

        public StandardVictoryPoint CardVictoryPoints { get; set; }

        public ResourcesVictoryPoints CardResourcesVictoryPoints { get; set; } 

        public MineralModifiers MineralModifiers { get; set; }

        public ConversionRates ConversionRates { get; set; } 

        public List<Effect> AllEffects()
        {
            return new List<Effect>(ResourcesEffects)
                .Concat(TagEffects != null && TagEffects.Any() ? TagEffects.Cast<Effect>() : new List<Effect>())
                .Concat(MineralModifiers?.SteelModifier != null ? new List<Effect> { MineralModifiers.SteelModifier } : new List<Effect>())
                .Concat(MineralModifiers?.TitaniumModifier != null ? new List<Effect> { MineralModifiers.TitaniumModifier } : new List<Effect>())
                .Concat(ConversionRates?.Heat != null ? new List<Effect> { ConversionRates.Heat } : new List<Effect>())
                .Concat(ConversionRates?.PlantConversion != null ? new List<Effect> { ConversionRates.PlantConversion } : new List<Effect>())

                .Concat(BoardEffects)
                .Concat((TilePassiveEffects != null && TilePassiveEffects.Any()? TilePassiveEffects.Cast<Effect>():new List<Effect>()))

                .Concat(TileEffects)
                .Concat((Actions != null && Actions.Any()) ? Actions.Cast<Effect>() : new List<Effect>())
                .ToList();
        }
    }

    public class ConversionRates
    {
        public ConversionRate PlantConversion { get; set; }
        public ConversionRate Heat { get; set; }
    }

    public class ConversionRate : Effect
    {
        public ResourceType ResourceType { get; set; }
        public int Rate { get; set; }
    }

    public class MineralModifiers
    {
        public MineralModifier SteelModifier { get; set; }

        public MineralModifier TitaniumModifier
        { get; set; }
    }

    public class MineralModifier : Effect
    {
        public ResourceType ResourceType { get; set; }
        public int Value { get; set; }
    }

    public class TagsEffects : List<TagEffect>
    {
    }

    public class Action : Effect
    {
        public ActionFrom ActionFrom { get; set; }

        public List<ActionTo> ActionTo { get; set; }

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

        public BoardLevelType BoardLevelType { get; set; }

        public int Amount { get; set; }
        public ResourceKind ResourceKind { get; set; }
    }

    public enum ActionTarget
    {
        Self,
        CurrentCard,
        AnyOtherCard,
        AnyPlayer,
        AnyOpponent
    }
}