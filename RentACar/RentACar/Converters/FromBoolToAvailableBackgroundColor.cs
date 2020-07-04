using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentACar.Converters
{
    class FromBoolToAvailableBackgroundColor : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Xamarin.Forms.Color.Green;
            }
            else
                return Xamarin.Forms.Color.Red;
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
