// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Threading.Tasks;
    using Mvvm.Commands;
    using Services;

    public abstract class NavigateCommandBase : ProgressingAsyncCommandBase
    {
        protected NavigateCommandBase(IDialogService dialogService, INavigationService navigationService)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            if (navigationService == null)
                throw new ArgumentNullException(nameof(navigationService));
            NavigationService = navigationService;
            DialogService = dialogService;
        }

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }
    }

    public abstract class NavigateCommandBase<T> : ProgressingAsyncCommandBase<T>
    {
        protected NavigateCommandBase(IDialogService dialogService, INavigationService navigationService)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            if (navigationService == null)
                throw new ArgumentNullException(nameof(navigationService));
            DialogService = dialogService;
            NavigationService = navigationService;
        }

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        protected override async Task ExecuteCoreAsync(T parameter)
        {
            var navTuple = GetNavTuple(parameter);
            if (navTuple == null)
            {
                await
                    DialogService.ShowDialogAsync("View unavailable",
                        GetErrorString(parameter), "Oh well");
                return;
            }
            await NavigationService.NavigateAsync(navTuple.Page, navTuple.ViewModel);
        }

        protected virtual string GetErrorString(T parameter)
        {
            return $"Currently there is no implementation to open '{parameter}'";
        }

        protected abstract NavTuple GetNavTuple(T parameter);
    }
}
