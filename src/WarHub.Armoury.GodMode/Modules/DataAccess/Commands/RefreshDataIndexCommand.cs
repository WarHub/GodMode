// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.Repo;

    public class RefreshDataIndexCommand : AppAsyncCommandBase
    {
        public RefreshDataIndexCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IDataIndexService dataIndexService) : base(dependencyAggregate)
        {
            DataIndexService = dataIndexService;
            IsExecutionBlocking = true;
        }

        private IDataIndexService DataIndexService { get; }

        protected override async Task ExecuteCoreAsync()
        {
            await DataIndexService.IndexStorageAsync();
        }
    }
}
