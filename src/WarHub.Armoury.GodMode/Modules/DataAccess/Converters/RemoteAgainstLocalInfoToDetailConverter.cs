// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Converters
{
    using System;
    using System.Globalization;
    using ViewModels;
    using Xamarin.Forms;

    public class RemoteAgainstLocalInfoToDetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var updateInfo = (RemoteAgainstLocalInfo) value;
            if (updateInfo == null)
            {
                return null;
            }
            return updateInfo.IsNewFile
                ? $"(none) -> v{updateInfo.RemoteRevision}"
                : updateInfo.LocalRevision == updateInfo.RemoteRevision
                    ? $"v{updateInfo.LocalRevision} -> v{updateInfo.RemoteRevision} (no update)"
                    : $"v{updateInfo.LocalRevision} -> v{updateInfo.RemoteRevision}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
