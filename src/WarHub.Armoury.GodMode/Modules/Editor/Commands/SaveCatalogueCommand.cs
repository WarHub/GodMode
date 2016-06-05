// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.Repo;

    public class SaveCatalogueCommand : AppAsyncCommandBase<CatalogueInfo>
    {
        public SaveCatalogueCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRepoManagerLocator repoManagerLocator) : base(dependencyAggregate)
        {
            RepoManagerLocator = repoManagerLocator;
            IsExecutionBlocking = true;
        }

        private IRepoManagerLocator RepoManagerLocator { get; }

        protected override async Task ExecuteCoreAsync(CatalogueInfo parameter)
        {
            var repoManager = RepoManagerLocator[parameter];
            await repoManager.SaveCatalogueAsync(parameter);
        }

        protected override bool CanExecuteCore(CatalogueInfo parameter) => parameter != null;
    }
}
