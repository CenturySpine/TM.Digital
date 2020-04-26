using System.Collections.Generic;
using TM.Digital.Model.Effects;

namespace TM.Digital.Model.Cards
{
    public class Action : Effect
    {
        public ActionFrom ActionFrom { get; set; }

        public List<ActionTo> ActionTo { get; set; }

        public ActionModifier ActionModifier { get; set; }

        public bool CanExecute { get; set; }
    }
}