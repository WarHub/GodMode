// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.Repo;

    public class RemoveCatalogueCommand : AppAsyncCommandBase<CatalogueInfo>
    {
        public RemoveCatalogueCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRepoStorageService repoStorageService) : base(dependencyAggregate)
        {
            RepoStorageService = repoStorageService;
        }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(CatalogueInfo parameter)
        {
            await RepoStorageService.DeleteCatalogueAsync(parameter);
        }
    }
}
