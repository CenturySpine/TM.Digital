using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TM.Digital.Model.Cards;

namespace TM.Digital.Ui.Resources.Resources.Converters
{
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
}