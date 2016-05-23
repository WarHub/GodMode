// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System;
    using System.Linq;
    using Commands;
    using Model.Repo;
    using Mvvm;

    public delegate RemoteDataUpdateInfoViewModel RemoteDataUpdateInfoVmFactory(
        RemoteDataInfo remoteDataInfo, Uri indexUri);

    /// <summary>
    ///     Shows information about available update, ie. whether it's a new file, or it has new revision available.
    /// </summary>
    public class RemoteDataUpdateInfoViewModel : ViewModelBase
    {
        public RemoteDataUpdateInfoViewModel(RemoteDataInfo remoteDataInfo, Uri indexUri,
            IDataIndexService dataIndexService, DownloadSingleDataItemCommand downloadCommand)
        {
            RemoteDataInfo = remoteDataInfo;
            IndexUri = indexUri;
            DownloadCommand = downloadCommand;
            switch (RemoteDataInfo.DataType)
            {
                case RemoteDataType.Catalogue:
                {
                    var localInfo =
                        dataIndexService.SystemIndexes.SelectMany(index => index.CatalogueInfos)
                            .FirstOrDefault(catalogueInfo => catalogueInfo.RawId == RemoteDataInfo.RawId);
                    IsNewFile = localInfo == null;
                    IsUpdate = localInfo != null && RemoteDataInfo.Revision > localInfo.Revision;
                    LocalRevision = localInfo?.Revision;
                }
                    break;
                case RemoteDataType.GameSystem:
                {
                    var localInfo =
                        dataIndexService.SystemIndexes.Where(index => index.GameSystemInfo != null)
                            .Select(index => index.GameSystemInfo)
                            .FirstOrDefault(systemInfo => systemInfo.RawId == RemoteDataInfo.RawId);
                    IsNewFile = localInfo == null;
                    IsUpdate = localInfo != null && RemoteDataInfo.Revision > localInfo.Revision;
                    LocalRevision = localInfo?.Revision;
                }
                    break;
                default:
                    break;
            }
        }

        public Uri IndexUri { get; }

        public bool IsNewFile { get; }

        public bool IsUpdate { get; }

        public uint? LocalRevision { get; }

        public RemoteDataInfo RemoteDataInfo { get; }

        public DownloadSingleDataItemCommand DownloadCommand { get; }
    }
}
