using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Converters
{
    public class StringEqualConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Equals(parameter?.ToString()) ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Equals(parameter?.ToString()) ?? false;
        }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
