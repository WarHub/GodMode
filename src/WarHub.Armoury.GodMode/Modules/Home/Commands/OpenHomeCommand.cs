// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class OpenHomeCommand : NavigateCommandBase
    {
        public OpenHomeCommand(IAppCommandDependencyAggregate dependencyAggregate, INavigationService navigationService,
            Func<MainViewModel> mainVmFactory) : base(dependencyAggregate, navigationService)
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
