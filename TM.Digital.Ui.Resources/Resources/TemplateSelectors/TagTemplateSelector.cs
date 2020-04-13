using System;
using System.Windows;
using System.Windows.Controls;
using TM.Digital.Model.Cards;

namespace TM.Digital.Ui.Resources.Resources.TemplateSelectors
{
    public class TagTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Animal { get; set; }
        public DataTemplate Plant { get; set; }

        public DataTemplate Building { get; set; }

        public DataTemplate City { get; set; }
        public DataTemplate Event { get; set; }
        public DataTemplate Space { get; set; }
        public DataTemplate Earth { get; set; }
        public DataTemplate Jupiter { get; set; }
        public DataTemplate Microbe { get; set; }
        public DataTemplate Science { get; set; }
        public DataTemplate Energy { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                Tags t = (Tags)Enum.Parse(typeof(Tags), item.ToString());
                switch (t)
                {
                    case Tags.Building:
                        return Building;

                    case Tags.Event:
                        return Event;

                    case Tags.Space:
                        return Space;

                    case Tags.Microbe:
                        return Microbe;

                    case Tags.Plant:
                        return Plant;

                    case Tags.Animal:
                        return Animal;

                    case Tags.Energy:
                        return Energy;

                    case Tags.Jupiter:
                        return Jupiter;

                    case Tags.Earth:
                        return Earth;

                    case Tags.City:
                        return City;

                    case Tags.Science:
                        return Science;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }
}