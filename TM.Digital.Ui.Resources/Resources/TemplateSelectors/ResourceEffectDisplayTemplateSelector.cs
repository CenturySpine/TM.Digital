using System.Windows;
using System.Windows.Controls;
using TM.Digital.Model.Effects;
using TM.Digital.Model.Resources;

namespace TM.Digital.Ui.Resources.Resources.TemplateSelectors
{
    public class ResourceEffectDisplayTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Standard { get; set; }
        public DataTemplate Money { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is ResourceEffect rh)
            {
                if (rh.ResourceType == ResourceType.Money)
                    return Money;
                return Standard;
            }

            return null;
        }
    }
}