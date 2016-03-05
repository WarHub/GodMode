// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Converters
{
    using System;
    using System.Globalization;
    using Model;
    using Models;
    using Xamarin.Forms;

    internal class ModifierToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as IEntryModifier)?.Stringify() ?? (value as IGroupModifier)?.Stringify() ??
                   (value as IProfileModifier)?.Stringify() ?? (value as IRuleModifier)?.Stringify();
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
