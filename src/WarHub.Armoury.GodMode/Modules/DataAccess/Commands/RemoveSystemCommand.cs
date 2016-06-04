// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.Repo;

    public class RemoveSystemCommand : AppAsyncCommandBase<ISystemIndex>
    {
        public RemoveSystemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRepoManagerLocator repoManagerLocator, IRepoStorageService repoStorageService) : base(dependencyAggregate)
        {
            RepoManagerLocator = repoManagerLocator;
            RepoStorageService = repoStorageService;
        }

        private IRepoManagerLocator RepoManagerLocator { get; }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(ISystemIndex parameter)
        {
            var isAccepted = await DialogService.ShowDialogAsync("Are you sure?",
                "Deleting system data is permanent and unrecoverable. You'll loose all catalogues and game system files for this game system that you've imported into GodMode.",
                "yes, delete", "cancel");
            if (!isAccepted)
            {
                return;
            }
            try
            {
                var repoManager = parameter.GameSystemInfo != null
                    ? RepoManagerLocator[parameter.GameSystemInfo]
                    : RepoManagerLocator[parameter.CatalogueInfos.First()];
                RepoManagerLocator.Deregister(repoManager);
                repoManager.ClearCache();
            }
            catch (Exception e)
            {
                Log.Info?.With($"Couldn't unregister repo manager, system raw id='{parameter.GameSystemRawId}'", e);
            }
            if (parameter.GameSystemInfo != null)
            {
                await RepoStorageService.DeleteGameSystemAsync(parameter.GameSystemInfo);
            }
            else
            {
                foreach (var catalogueInfo in parameter.CatalogueInfos)
                {
                    await RepoStorageService.DeleteCatalogueAsync(catalogueInfo);
                }
            }
        }

        protected override bool CanExecuteCore(ISystemIndex parameter) => parameter != null;
    }
}
