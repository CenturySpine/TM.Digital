using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Resources;

namespace TM.Digital.Client
{
    public class TagsCount
    {
        public Tags Tag { get; set; }
        public int Count { get; set; }
    }

    public class PlayedCardToTagsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cards = (List<Patent>)value;
            var lst = cards.SelectMany(c => c.Tags).GroupBy(c => c)
                .Select(t => new TagsCount { Tag = t.Key, Count = t.Count() });
            return lst;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    public class ResourceBoardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Steel { get; set; }
        public DataTemplate Titanium { get; set; }
        public DataTemplate Plant { get; set; }
        public DataTemplate Money { get; set; }
        public DataTemplate Heat { get; set; }
        public DataTemplate Energy { get; set; }
        public DataTemplate Card { get; set; }

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
                        break;

                    case ResourceType.Microbe:
                        break;

                    case ResourceType.Fleet:
                        break;

                    case ResourceType.Science:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }
}