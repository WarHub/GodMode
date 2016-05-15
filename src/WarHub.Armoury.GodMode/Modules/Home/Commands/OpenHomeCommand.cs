// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.Commands
{
    using System;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    public class OpenHomeCommand : NavigateCommandBase
    {
        public OpenHomeCommand(IDialogService dialogService, INavigationService navigationService,
            Func<MainViewModel> mainVmFactory) : base(dialogService, navigationService)
        {
            MainVmFactory = mainVmFactory;
        }

        private Func<MainViewModel> MainVmFactory { get; }

        protected override async Task ExecuteCoreAsync()
        {
            await NavigationService.NavigateAsync(new MainPage(), MainVmFactory());
        }
    }
}
