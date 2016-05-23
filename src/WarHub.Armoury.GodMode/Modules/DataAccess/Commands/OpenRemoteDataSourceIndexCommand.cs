// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using Model.DataAccess;
    using ViewModels;
    using Views;

    public class OpenRemoteDataSourceIndexCommand : NavigateCommandBase<RemoteDataSourceInfo>
    {
        public OpenRemoteDataSourceIndexCommand(IDialogService dialogService, INavigationService navigationService,
            RemoteDataSourceIndexVmFactory remoteDataSourceIndexVmFactory, IRemoteDataService remoteDataService)
            : base(dialogService, navigationService)
        {
            RemoteDataSourceIndexVmFactory = remoteDataSourceIndexVmFactory;
            RemoteDataService = remoteDataService;
        }

        private IRemoteDataService RemoteDataService { get; }

        private RemoteDataSourceIndexVmFactory RemoteDataSourceIndexVmFactory { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataSourceInfo parameter)
        {
            var index = await RemoteDataService.DownloadIndexAsync(parameter);
            var vm = RemoteDataSourceIndexVmFactory(index);
            var navTuple = new NavTuple(new RemoteDataSourceIndexPage(), vm);
            await NavigateAsync(navTuple);
        }

        protected override bool CanExecuteCore(RemoteDataSourceInfo parameter)
        {
            return parameter != null;
        }
    }
}
