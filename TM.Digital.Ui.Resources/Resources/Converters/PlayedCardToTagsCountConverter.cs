using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Player;

namespace TM.Digital.Ui.Resources.Resources.Converters
{
    public class PlayedCardToTagsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var player = (Player)value;
            if (player != null)
            {
                var cards = player.PlayedCards.Concat(new List<Card>() {player.Corporation})
                    .Where(c => c.CardType != CardType.Red);
                var lst = cards.SelectMany(c => c.Tags).GroupBy(c => c)
                    .Select(t => new TagsCount { Tag = t.Key, Count = t.Count() });
                return lst;
            }
            return new List<TagsCount>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}