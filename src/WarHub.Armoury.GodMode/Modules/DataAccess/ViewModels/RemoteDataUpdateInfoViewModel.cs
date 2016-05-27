// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Input;
    using Commands;
    using Model.Repo;
    using Mvvm;

    public delegate RemoteDataUpdateInfoViewModel RemoteDataUpdateInfoVmFactory(
        RemoteDataInfo remoteDataInfo, Uri indexUri);

    /// <summary>
    ///     Shows information about available update, ie. whether it's a new file, or it has new revision available.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RemoteDataUpdateInfoViewModel : ViewModelBase
    {
        private RemoteAgainstLocalInfo _remoteAgainstLocalInfo;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public RemoteDataUpdateInfoViewModel(RemoteDataInfo remoteDataInfo, Uri indexUri,
            IDataIndexService dataIndexService, DownloadSingleDataItemCommand downloadCommand)
        {
            RemoteDataInfo = remoteDataInfo;
            IndexUri = indexUri;
            DataIndexService = dataIndexService;
            DownloadCommand = downloadCommand;
            UpdateLocalInfo();
        }

        public string DataName => RemoteDataInfo.Name;

        public ICommand DownloadCommand { get; }

        public Uri IndexUri { get; }

        public RemoteAgainstLocalInfo RemoteAgainstLocalInfo
        {
            get { return _remoteAgainstLocalInfo; }
            private set { Set(ref _remoteAgainstLocalInfo, value); }
        }

        public RemoteDataInfo RemoteDataInfo { get; }

        private IDataIndexService DataIndexService { get; }

        public void UpdateLocalInfo()
        {
            switch (RemoteDataInfo.DataType)
            {
                case RemoteDataType.Catalogue:
                {
                    var localInfo =
                        DataIndexService.SystemIndexes.SelectMany(index => index.CatalogueInfos)
                            .FirstOrDefault(catalogueInfo => catalogueInfo.RawId == RemoteDataInfo.RawId);
                    RemoteAgainstLocalInfo = new RemoteAgainstLocalInfo.Builder
                    {
                        IsNewFile = localInfo == null,
                        IsUpdate = localInfo != null && RemoteDataInfo.Revision > localInfo.Revision,
                        LocalRevision = localInfo?.Revision,
                        RemoteRevision = RemoteDataInfo.Revision
                    };
                }
                    break;
                case RemoteDataType.GameSystem:
                {
                    var localInfo =
                        DataIndexService.SystemIndexes.Where(index => index.GameSystemInfo != null)
                            .Select(index => index.GameSystemInfo)
                            .FirstOrDefault(systemInfo => systemInfo.RawId == RemoteDataInfo.RawId);
                    RemoteAgainstLocalInfo = new RemoteAgainstLocalInfo.Builder
                    {
                        IsNewFile = localInfo == null,
                        IsUpdate = localInfo != null && RemoteDataInfo.Revision > localInfo.Revision,
                        LocalRevision = localInfo?.Revision,
                        RemoteRevision = RemoteDataInfo.Revision
                    };
                }
                    break;
                default:
                    break;
            }
        }
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RemoteAgainstLocalInfo
    {
        public RemoteAgainstLocalInfo(bool isNewFile, bool isUpdate, uint? localRevision, uint? remoteRevision)
        {
            IsNewFile = isNewFile;
            IsUpdate = isUpdate;
            LocalRevision = localRevision;
            RemoteRevision = remoteRevision;
        }

        public bool IsNewFile { get; }

        public bool IsUpdate { get; }

        public uint? LocalRevision { get; }

        public uint? RemoteRevision { get; }

        public class Builder
        {
            public bool IsNewFile { get; set; }

            public bool IsUpdate { get; set; }

            public uint? LocalRevision { get; set; }

            public uint? RemoteRevision { get; set; }

            public static implicit operator RemoteAgainstLocalInfo(Builder @this)
            {
                return new RemoteAgainstLocalInfo(@this.IsNewFile, @this.IsUpdate, @this.LocalRevision,
                    @this.RemoteRevision);
            }
        }
    }
}
