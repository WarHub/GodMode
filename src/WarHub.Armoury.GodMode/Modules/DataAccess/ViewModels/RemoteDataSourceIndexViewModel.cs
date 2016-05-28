// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Bindables;
    using Commands;
    using Model.Repo;
    using Mvvm;

    public delegate RemoteDataSourceIndexViewModel RemoteDataSourceIndexVmFactory(
        RemoteSourceDataIndex remoteSourceDataIndex);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoteDataSourceIndexViewModel : ViewModelBase
    {
        public RemoteDataSourceIndexViewModel(RemoteSourceDataIndex remoteSourceDataIndex,
            RemoteDataUpdateInfoVmFactory remoteDataUpdateInfoVmFactory, DownloadDataSourceCommand downloadCommand)
        {
            RemoteSourceDataIndex = remoteSourceDataIndex;
            DownloadCommand = downloadCommand;
            GameSystemUpdateInfoViewModels = RemoteSourceDataIndex.RemoteDataInfos
                .Where(info => info.DataType == RemoteDataType.GameSystem)
                .OrderBy(info => info.Name)
                .Select(info => remoteDataUpdateInfoVmFactory(info, RemoteSourceDataIndex.IndexUri))
                .ToArray()
                .ToBindableGrouping("Game systems", "gst");
            CatalogueUpdateInfoViewModels = RemoteSourceDataIndex.RemoteDataInfos
                .Where(info => info.DataType == RemoteDataType.Catalogue)
                .OrderBy(info => info.Name)
                .Select(info => remoteDataUpdateInfoVmFactory(info, RemoteSourceDataIndex.IndexUri))
                .ToArray()
                .ToBindableGrouping("Catalogues", "cat");
        }

        public IBindableGrouping<RemoteDataUpdateInfoViewModel> CatalogueUpdateInfoViewModels { get; }

        public DownloadDataSourceCommand DownloadCommand { get; }

        public IBindableGrouping<RemoteDataUpdateInfoViewModel> GameSystemUpdateInfoViewModels { get; }

        public IEnumerable<IBindableGrouping<RemoteDataUpdateInfoViewModel>> UpdateInfos
        {
            get
            {
                yield return GameSystemUpdateInfoViewModels;
                yield return CatalogueUpdateInfoViewModels;
            }
        }

        private RemoteSourceDataIndex RemoteSourceDataIndex { get; }
    }
}
