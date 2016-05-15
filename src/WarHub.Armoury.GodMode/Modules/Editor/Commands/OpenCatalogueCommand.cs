// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using AppServices;
    using Demo;
    using GodMode.Commands;
    using Model;
    using Model.Repo;
    using ViewModels;
    using Views;

    public class OpenCatalogueCommand : NavigateCommandBase<CatalogueInfo>
    {
        public OpenCatalogueCommand(IDialogService dialogService, INavigationService navigationService,
            Func<ICatalogue, CatalogueViewModel> catalogueVmFactory)
            : base(dialogService, navigationService)
        {
            CatalogueVmFactory = catalogueVmFactory;
        }

        private Func<ICatalogue, CatalogueViewModel> CatalogueVmFactory { get; }

        protected override NavTuple GetNavTuple(CatalogueInfo parameter)
        {
            // TODO load catalogue from parameter
            return new NavTuple(new CataloguePage(), CatalogueVmFactory(ModelLocator.Catalogue));
        }

        protected override string GetErrorString(CatalogueInfo parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(CatalogueInfo parameter) => parameter != null;
    }
}
