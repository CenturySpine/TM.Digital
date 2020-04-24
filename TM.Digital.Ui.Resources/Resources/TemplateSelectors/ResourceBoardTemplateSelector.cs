using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TM.Digital.Model.Resources;

namespace TM.Digital.Ui.Resources.Resources.TemplateSelectors
{
    public class ResourceBoardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Steel { get; set; }
        public DataTemplate Titanium { get; set; }
        public DataTemplate Plant { get; set; }
        public DataTemplate Money { get; set; }
        public DataTemplate Heat { get; set; }
        public DataTemplate Energy { get; set; }
        public DataTemplate Card { get; set; }
        public DataTemplate Animal { get; set; }
        public DataTemplate Microbe { get; set; }
        public DataTemplate Science { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                ResourceType t = (ResourceType)Enum.Parse(typeof(ResourceType), item.ToString());
                switch (t)
                {
                    case ResourceType.None:
                        break;

                    case ResourceType.Steel:
                        return Steel;

                    case ResourceType.Titanium:
                        return Titanium;

                    case ResourceType.Card:
                        return Card;

                    case ResourceType.Plant:
                        return Plant;

                    case ResourceType.Heat:
                        return Heat;

                    case ResourceType.Money:
                        return Money;

                    case ResourceType.Energy:
                        return Energy;

                    case ResourceType.Animal:
                        return Animal;

                    case ResourceType.Microbe:
                        return Microbe;

                    case ResourceType.Fleet:
                        break;

                    case ResourceType.Science:
                        return Science;

                    default:
                        return null;
                }
            }

            return null;
        }
    }
}