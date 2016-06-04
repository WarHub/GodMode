// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.DataAccess;

    public class RefreshRemoteDataSourcesCommand : AppAsyncCommandBase
    {
        public RefreshRemoteDataSourcesCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRemoteSourceIndexService remoteSourceIndexService) : base(dependencyAggregate)
        {
            RemoteSourceIndexService = remoteSourceIndexService;
            IsExecutionBlocking = true;
        }

        private IRemoteSourceIndexService RemoteSourceIndexService { get; }

        protected override async Task ExecuteCoreAsync()
        {
            await RemoteSourceIndexService.ReloadIndexAsync();
        }
    }
}
