using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Effects;

namespace TM.Digital.Model.Corporations
{
    public class Corporation : Card
    {
        [XmlIgnore]
        [JsonIgnore]
        public Effect CorporationEffect
        {
            get { return AllEffects().FirstOrDefault(e => e.IsCorporationEffect); }
        }
    }
}
