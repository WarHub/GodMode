// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    public class OpenRemoteDataIndexCommand : NavigateCommandBase
    {
        public OpenRemoteDataIndexCommand(IDialogService dialogService, INavigationService navigationService,
            Func<RemoteDataIndexViewModel> remoteDataIndexVmFactory) : base(dialogService, navigationService)
        {
            RemoteDataIndexVmFactory = remoteDataIndexVmFactory;
        }

        private Func<RemoteDataIndexViewModel> RemoteDataIndexVmFactory { get; }

        protected override async Task ExecuteCoreAsync()
        {
            var vm = RemoteDataIndexVmFactory();
            await NavigationService.NavigateAsync(new RemoteDataIndexPage(), vm);
        }
    }
}
