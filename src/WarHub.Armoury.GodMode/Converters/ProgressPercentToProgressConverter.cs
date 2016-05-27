// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Converters
{
    using System;
    using System.Globalization;
    using Mvvm;
    using Xamarin.Forms;

    /// <summary>
    ///     Use to convert <see cref="ProgressReportingBase.ProgressPercent" /> (and variant classes) to
    ///     <see cref="ProgressBar.Progress" /> value.
    /// </summary>
    public class ProgressPercentToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var progressPercent = value as int?;
            return progressPercent/100d ?? 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
