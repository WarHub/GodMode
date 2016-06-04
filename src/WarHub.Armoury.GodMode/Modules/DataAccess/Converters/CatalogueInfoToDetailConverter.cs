// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Converters
{
    using System;
    using System.Globalization;
    using Model.Repo;
    using Xamarin.Forms;

    public class CatalogueInfoToDetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var catalogueInfo = value as CatalogueInfo;
            if (catalogueInfo == null)
            {
                return null;
            }
            return $"v{catalogueInfo.Revision}, by {catalogueInfo.AuthorName}, id='{catalogueInfo.RawId}'";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
