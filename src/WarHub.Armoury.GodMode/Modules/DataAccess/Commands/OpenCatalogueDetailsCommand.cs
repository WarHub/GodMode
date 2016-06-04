// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using AppServices;
    using GodMode.Commands;
    using Model.Repo;
    using ViewModels;
    using Views;

    public class OpenCatalogueDetailsCommand : NavigateCommandBase<CatalogueInfo>
    {
        public OpenCatalogueDetailsCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, CatalogueDetailsVmFactory catalogueDetailsVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            CatalogueDetailsVmFactory = catalogueDetailsVmFactory;
        }

        private CatalogueDetailsVmFactory CatalogueDetailsVmFactory { get; }

        protected override NavTuple GetNavTuple(CatalogueInfo parameter)
        {
            var vm = CatalogueDetailsVmFactory(parameter);
            return new NavTuple(new CatalogueDetailsPage(), vm);
        }

        protected override bool CanExecuteCore(CatalogueInfo parameter) => parameter != null;
    }
}
