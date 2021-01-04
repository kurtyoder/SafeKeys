using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace SafeKeys.ValueConverters
{
    public class BooleanVisiblityConverter : BaseValueConverter<BooleanVisiblityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter == null
                ? (bool)value ? Visibility.Hidden : Visibility.Visible
                : (object)((bool)value ? Visibility.Visible : Visibility.Hidden);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}