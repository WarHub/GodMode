// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    public class OpenRemoteDataSourcesCommand : NavigateCommandBase
    {
        public OpenRemoteDataSourcesCommand(IDialogService dialogService, INavigationService navigationService,
            Func<RemoteDataSourcesViewModel> remoteDataIndexVmFactory) : base(dialogService, navigationService)
        {
            RemoteDataIndexVmFactory = remoteDataIndexVmFactory;
        }

        private Func<RemoteDataSourcesViewModel> RemoteDataIndexVmFactory { get; }

        protected override NavTuple GetNavTuple()
        {
            var vm = RemoteDataIndexVmFactory();
            return new NavTuple(new RemoteDataSourcesPage(), vm);
        }
    }
}
