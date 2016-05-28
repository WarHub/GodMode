// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using GodMode.Commands;
    using Model.DataAccess;
    using Model.Repo;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoveDataSourceCommand : AppCommandBase<RemoteSource>
    {
        public RemoveDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRemoteSourceIndexService remoteSourceIndexService) : base(dependencyAggregate)
        {
            RemoteSourceIndexService = remoteSourceIndexService;
        }

        private IRemoteSourceIndexService RemoteSourceIndexService { get; }

        protected override void ExecuteCore(RemoteSource parameter)
        {
            RemoteSourceIndexService.RemoveSource(parameter);
        }

        protected override bool CanExecuteCore(RemoteSource parameter) => parameter != null;
    }
}
