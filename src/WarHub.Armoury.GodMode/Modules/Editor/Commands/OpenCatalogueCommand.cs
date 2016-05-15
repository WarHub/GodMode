// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using AppServices;
    using Demo;
    using GodMode.Commands;
    using Model.Repo;
    using ViewModels;
    using Views;

    public class OpenCatalogueCommand : NavigateCommandBase<CatalogueInfo>
    {
        public OpenCatalogueCommand(IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
        }

        protected override NavTuple GetNavTuple(CatalogueInfo parameter)
        {
            return new NavTuple(new CataloguePage(),
                ViewModelLocator.CatalogueViewModel.WithModel(ModelLocator.Catalogue));
        }

        protected override string GetErrorString(CatalogueInfo parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(CatalogueInfo parameter) => parameter != null;
    }
}
