using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using TM.Digital.Model.Board;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;
using Action = TM.Digital.Model.Cards.Action;

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
        public List<BoardAction> BoardActions { get; set; }

        public ResourceHandler this[ResourceType r]
        {
            get { return Resources.First(h => h.ResourceType == r); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<Action> AllPlayerActions
        {
            get
            {
                return
                    PlayedCards.Where(c=>c.Actions != null && c.Actions.Any()).SelectMany(pc => pc.Actions).ToList();
            }
        }

    }
}