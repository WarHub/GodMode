// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using Editor.Commands;
    using Model.Repo;
    using Mvvm;

    public delegate CatalogueDetailsViewModel CatalogueDetailsVmFactory(CatalogueInfo catalogueInfo);

    public class CatalogueDetailsViewModel : ViewModelBase
    {
        public CatalogueDetailsViewModel(CatalogueInfo catalogueInfo, OpenCatalogueCommand openCatalogueCommand)
        {
            CatalogueInfo = catalogueInfo;
            OpenCatalogueCommand = openCatalogueCommand;
        }

        public CatalogueInfo CatalogueInfo { get; }

        public OpenCatalogueCommand OpenCatalogueCommand { get; }
    }
}
