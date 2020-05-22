using System;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;

namespace TM.Digital.Model.Player
{
    public class ResourceEffectPlayerChooser
    {
        public Guid TargetPlayerId { get; set; }
        public string TargetPlayerName { get; set; }

        public ResourceEffect ResourceHandler { get; set; }
        public int EffectValue { get; set; }

        public string CardName { get; set; }
        public Guid CardId { get; set; }
    }

    public class ResourceToOtherCardChooser
    {
        public ResourceEffect Effect { get; set; }
        public string CardName { get; set; }
        public int CardResourceCount { get; set; }
        public Card Card { get; set; }
    }
}