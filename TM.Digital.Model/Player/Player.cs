using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Model.Player
{
    public class Player
    {
        public Corporation Corporation { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int TerraformationLevel { get; set; }
        public List<ResourceHandler> Resources { get; set; } 

        public List<Patent> HandCards { get; set; }
        public List<Patent> PlayedCards { get; set; }
        public int RemainingActions { get; set; }
        public int TotalActions { get; set; }
        public bool IsReady { get; set; }

        public string Color { get; set; }

        public ResourceHandler this[ResourceType r]
        {
            get { return Resources.First(h => h.ResourceType == r); }
        }
    

    }

    public class ResourceEffectPlayerChooserList
    {
        public List<ResourceEffectPlayerChooser> ChoicesList { get; set; }
    }
    public class ResourceEffectPlayerChooser
    {
        public Guid TargetPlayerId { get; set; }
        public string TargetPlayerName { get; set; }

        public ResourceEffect ResourceHandler { get; set; }
        public int EffectValue { get; set; }

        public string CardName { get; set; }
        public Guid CardId { get; set; }
    }
}