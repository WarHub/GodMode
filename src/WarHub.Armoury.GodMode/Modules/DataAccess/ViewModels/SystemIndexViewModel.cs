// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using Commands;
    using Model;
    using Model.Repo;
    using Mvvm;

    public delegate SystemIndexViewModel SystemIndexVmFactory(ISystemIndex systemIndex);

    public class SystemIndexViewModel : ViewModelBase
    {
        public SystemIndexViewModel(ISystemIndex systemIndex, RemoveCatalogueCommand removeCatalogueCommand,
            OpenCatalogueDetailsCommand openCatalogueDetailsCommand,
            OpenGameSystemDetailsCommand openGameSystemDetailsCommand)
        {
            SystemIndex = systemIndex;
            RemoveCatalogueCommand = removeCatalogueCommand;
            OpenCatalogueDetailsCommand = openCatalogueDetailsCommand;
            OpenGameSystemDetailsCommand = openGameSystemDetailsCommand;
        }

        public IObservableReadonlySet<CatalogueInfo> CatalogueInfos => SystemIndex.CatalogueInfos;

        public GameSystemInfo GameSystemInfo => SystemIndex.GameSystemInfo;

        public OpenCatalogueDetailsCommand OpenCatalogueDetailsCommand { get; }

        public OpenGameSystemDetailsCommand OpenGameSystemDetailsCommand { get; }

        public RemoveCatalogueCommand RemoveCatalogueCommand { get; }

        private ISystemIndex SystemIndex { get; }
    }
}
