using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace SafeKeys.ValueConverters
{
    /// <summary>
    /// Base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region Private Fields

        /// <summary>
        /// Single static instance of this value converter
        /// </summary>
        private static T converter = null;

        #endregion Private Fields

        #region Markup Extension Methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new T());
        }

        #endregion Markup Extension Methods

        #region Value Converter Methods

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion Value Converter Methods
    }
}