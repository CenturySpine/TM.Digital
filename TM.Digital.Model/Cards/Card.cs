using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        [Category("Description")]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [Category("Description")]
        public string Name { get; set; }

        [Category("Description")]
        public string OfficialNumberTag { get; set; }

        [Category("Description")]
        public CardType CardType { get; set; }

        [Category("Description")]
        public TagsList Tags { get; set; }

        [Category("Effect")]
        public List<ResourceEffect> ResourcesEffects { get; set; } = new List<ResourceEffect>();

        [Category("Effect")]
        public List<ResourceEffect> ResourceEffectAlternatives { get; set; }

        [Category("Effect")]
        public List<BoardLevelEffect> BoardEffects { get; set; } = new List<BoardLevelEffect>();

        [Category("Effect")]
        public TagsEffects TagEffects { get; set; } = new TagsEffects();

        [Category("Effect")]
        public List<TileEffect> TileEffects { get; set; } = new List<TileEffect>();

        [Category("Effect")]
        public TilePassiveEffects TilePassiveEffects { get; set; } = new TilePassiveEffects();

        [Category("Action")]
        public List<Action> Actions { get; set; } = new List<Action>();

        public ResourceType ResourceType { get; set; }

        [Browsable(false)]
        public int ResourcesCount { get; set; }

        [Category("Victory Points")]
        public StandardVictoryPoint CardVictoryPoints { get; set; }

        [Category("Victory Points")]
        public ResourcesVictoryPoints CardResourcesVictoryPoints { get; set; }

        [Browsable(false)]
        [Category("Resources Modifiers")]
        public MineralModifiers MineralModifiers { get; set; }

        [Browsable(false)]
        [Category("Resources Modifiers")]
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
                .Concat((TilePassiveEffects != null && TilePassiveEffects.Any() ? TilePassiveEffects.Cast<Effect>() : new List<Effect>()))

                .Concat(TileEffects)
                .Concat((Actions != null && Actions.Any()) ? Actions.Cast<Effect>() : new List<Effect>())
                .ToList();
        }

        public string Comment { get; set; }
        public string Description { get; set; }
    }
}