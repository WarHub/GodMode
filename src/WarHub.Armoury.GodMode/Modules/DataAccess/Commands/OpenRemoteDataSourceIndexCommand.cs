// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using Model.DataAccess;
    using Model.Repo;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class OpenRemoteDataSourceIndexCommand : NavigateCommandBase<RemoteSource>
    {
        public OpenRemoteDataSourceIndexCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, IRemoteSourceIndexService remoteDataService,
            RemoteDataSourceIndexVmFactory remoteDataSourceIndexVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            RemoteDataService = remoteDataService;
            RemoteDataSourceIndexVmFactory = remoteDataSourceIndexVmFactory;
        }

        private IRemoteSourceIndexService RemoteDataService { get; }

        private RemoteDataSourceIndexVmFactory RemoteDataSourceIndexVmFactory { get; }

        protected override async Task ExecuteCoreAsync(RemoteSource parameter)
        {
            var index = await RemoteDataService.DownloadIndexAsync(parameter);
            var vm = RemoteDataSourceIndexVmFactory(index);
            var navTuple = new NavTuple(new RemoteDataSourceIndexPage(), vm);
            await NavigateAsync(navTuple);
        }

        protected override bool CanExecuteCore(RemoteSource parameter)
        {
            return parameter != null;
        }
    }
}
