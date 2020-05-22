using System;
using System.Globalization;
using System.Windows.Data;

namespace TM.Digital.Ui.Resources.Resources.Converters
{
    public class ResourceNegativeAmountDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string rawValue = value.ToString();
                if (rawValue.Contains("-"))
                {
                    return rawValue.Replace("-", "");
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}