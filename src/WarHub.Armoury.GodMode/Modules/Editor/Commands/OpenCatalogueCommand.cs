// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using Model;
    using Model.Repo;
    using ViewModels;
    using Views;

    public class OpenCatalogueCommand : NavigateCommandBase<CatalogueInfo>
    {
        public OpenCatalogueCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<ICatalogue, CatalogueViewModel> catalogueVmFactory,
            IRepoManagerLocator repoManagerLocator)
            : base(dependencyAggregate, navigationService)
        {
            CatalogueVmFactory = catalogueVmFactory;
            RepoManagerLocator = repoManagerLocator;
            IsExecutionBlocking = true;
        }

        private Func<ICatalogue, CatalogueViewModel> CatalogueVmFactory { get; }

        private IRepoManagerLocator RepoManagerLocator { get; }

        protected override async Task ExecuteCoreAsync(CatalogueInfo parameter)
        {
            var manager = RepoManagerLocator[parameter];
            var catalogue = await manager.GetCatalogueAsync(parameter);
            var navTuple = new NavTuple(new CataloguePage(), CatalogueVmFactory(catalogue));
            await NavigateAsync(navTuple);
        }

        protected override string GetErrorString(CatalogueInfo parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(CatalogueInfo parameter) => parameter != null;
    }
}
