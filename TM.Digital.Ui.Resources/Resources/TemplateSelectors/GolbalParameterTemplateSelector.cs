using System;
using System.Windows;
using System.Windows.Controls;
using TM.Digital.Model.Board;
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
    public class GolbalParameterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Temprature { get; set; }
        public DataTemplate Oxygen { get; set; }
        public DataTemplate TerraformationLevel { get; set; }
        public DataTemplate Ocean { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                BoardLevelType t = (BoardLevelType)Enum.Parse(typeof(BoardLevelType), item.ToString());
                switch (t)
                {
                    case BoardLevelType.Temperature:
                        return Temprature;

                    case BoardLevelType.Oxygen:
                        return Oxygen;
                    case BoardLevelType.Terraformation:
                        return TerraformationLevel;
                    case BoardLevelType.Oceans:
                        return Ocean;
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}