// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using GodMode.Commands;
    using Model.DataAccess;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoveDataSourceCommand : AppCommandBase<RemoteDataSourceInfo>
    {
        public RemoveDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRemoteDataService remoteDataService) : base(dependencyAggregate)
        {
            RemoteDataService = remoteDataService;
        }

        private IRemoteDataService RemoteDataService { get; }

        protected override void ExecuteCore(RemoteDataSourceInfo parameter)
        {
            RemoteDataService.RemoveSource(parameter);
        }

        protected override bool CanExecuteCore(RemoteDataSourceInfo parameter) => parameter != null;
    }
}
