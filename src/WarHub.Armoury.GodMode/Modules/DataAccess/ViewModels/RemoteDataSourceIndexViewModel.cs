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
        RemoteDataSourceIndex remoteDataSourceIndex);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoteDataSourceIndexViewModel : ViewModelBase
    {
        public RemoteDataSourceIndexViewModel(RemoteDataSourceIndex remoteDataSourceIndex,
            RemoteDataUpdateInfoVmFactory remoteDataUpdateInfoVmFactory, DownloadDataSourceCommand downloadCommand)
        {
            RemoteDataSourceIndex = remoteDataSourceIndex;
            DownloadCommand = downloadCommand;
            GameSystemUpdateInfoViewModels = RemoteDataSourceIndex.RemoteDataInfos
                .Where(info => info.DataType == RemoteDataType.GameSystem)
                .OrderBy(info => info.Name)
                .Select(info => remoteDataUpdateInfoVmFactory(info, RemoteDataSourceIndex.IndexUri))
                .ToArray()
                .ToBindableGrouping("Game systems", "gst");
            CatalogueUpdateInfoViewModels = RemoteDataSourceIndex.RemoteDataInfos
                .Where(info => info.DataType == RemoteDataType.Catalogue)
                .OrderBy(info => info.Name)
                .Select(info => remoteDataUpdateInfoVmFactory(info, RemoteDataSourceIndex.IndexUri))
                .ToArray()
                .ToBindableGrouping("Catalogues", "cat");
        }

        public DownloadDataSourceCommand DownloadCommand { get; }

        public IEnumerable<IBindableGrouping<RemoteDataUpdateInfoViewModel>> UpdateInfos
        {
            get
            {
                yield return GameSystemUpdateInfoViewModels;
                yield return CatalogueUpdateInfoViewModels;
            }
        }

        public IBindableGrouping<RemoteDataUpdateInfoViewModel> CatalogueUpdateInfoViewModels { get; }

        public IBindableGrouping<RemoteDataUpdateInfoViewModel> GameSystemUpdateInfoViewModels { get; }

        private RemoteDataSourceIndex RemoteDataSourceIndex { get; }
    }
}
