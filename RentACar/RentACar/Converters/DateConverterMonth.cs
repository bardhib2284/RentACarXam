using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Converters
{
    public class DateConverterMonth : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                var g = dt.ToString("F", CultureInfo.CreateSpecificCulture("sq-AL"));
                var list = g.Split(',');
                var split = list[1].Split(' ');
                return (split[1] + " " + split[2]).ToUpper();
            }
            else
                return "GG";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
