// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Converters
{
    using System;
    using System.Globalization;
    using Bindables;
    using Model;
    using Xamarin.Forms;

    public class CatalogueConditionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var condition = (ICatalogueCondition) value;
            return condition == null ? string.Empty : condition.Stringify();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
