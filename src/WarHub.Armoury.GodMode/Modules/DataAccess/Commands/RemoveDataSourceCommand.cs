// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using Model.DataAccess;
    using Mvvm.Commands;

    public class RemoveDataSourceCommand : ProgressingCommandBase<RemoteDataSourceInfo>
    {
        public RemoveDataSourceCommand(IRemoteDataService remoteDataService)
        {
            RemoteDataService = remoteDataService;
        }

        private IRemoteDataService RemoteDataService { get; }

        protected override void ExecuteCore(RemoteDataSourceInfo parameter)
        {
            RemoteDataService.RemoveSource(parameter);
        }

        protected override bool CanExecuteCore(RemoteDataSourceInfo parameter)
        {
            return parameter != null;
        }
    }
}
