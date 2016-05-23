﻿// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Converters
{
    using System;
    using System.Globalization;
    using ViewModels;
    using Xamarin.Forms;

    internal class RemoteDataUpdateInfoToDetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var updateInfo = (RemoteDataUpdateInfoViewModel) value;
            return updateInfo == null
                ? null
                : $"{(updateInfo.IsNewFile ? "(none)" : $"v{updateInfo.LocalRevision}")} -> v{updateInfo.RemoteDataInfo.Revision}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
