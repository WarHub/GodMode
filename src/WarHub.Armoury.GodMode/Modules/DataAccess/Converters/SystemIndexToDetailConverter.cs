// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Model.Repo;
    using Xamarin.Forms;

    public class SystemIndexToDetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var systemIndex = value as ISystemIndex;
            if (systemIndex == null)
            {
                return null;
            }
            return $"{systemIndex.CatalogueInfos.Count()} catalogues, gst id='{systemIndex.GameSystemRawId}'";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
