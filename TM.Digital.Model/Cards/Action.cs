using System;
using System.Collections.Generic;
using System.ComponentModel;
using TM.Digital.Model.Effects;

namespace TM.Digital.Model.Cards
{
    public class Action : Effect
    {
        public ActionFrom ActionFrom { get; set; }

        public List<ActionTo> ActionTo { get; set; }

        public ActionModifier ActionModifier { get; set; }

        [Browsable(false)]
        public bool CanExecute { get; set; }

        [Browsable(false)]
        public Guid CardId { get; set; }

        [Browsable(false)]
        public bool Played { get; set; }
    }
}