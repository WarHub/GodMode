// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Threading.Tasks;
    using AppServices;
    using Mvvm.Commands;

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
            UseHandleExecutionException = true;
            RethrowExecutionException = false;
        }

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        protected override void HandleExecutionException(Exception e)
        {
            DialogService.ShowDialogAsync("Operation failed", e.Message, "Oh well");
            // TODO log exception?
        }

        protected override async Task ExecuteCoreAsync()
        {
            var navTuple = GetNavTuple();
            if (navTuple == null)
            {
                await
                    DialogService.ShowDialogAsync("View unavailable",
                        GetErrorString(), "Oh well");
                return;
            }
            await NavigateAsync(navTuple);
        }

        /// <summary>
        ///     Perform the navigation step or steps. Default implementation directly navigates to the page.
        /// </summary>
        /// <param name="navTuple"></param>
        /// <returns></returns>
        protected virtual async Task NavigateAsync(NavTuple navTuple)
        {
            await NavigationService.NavigateAsync(navTuple.Page, navTuple.ViewModel);
        }


        /// <summary>
        ///     Creates error message shown when <see cref="GetNavTuple" /> returns null.
        /// </summary>
        /// <returns>Error message</returns>
        protected virtual string GetErrorString()
        {
            return $"Currently there is no implementation to open requested view.";
        }

        /// <summary>
        ///     Create pair of navigation arguments for given parameter. May return null to show navigation error.
        /// </summary>
        /// <returns>Nav tuple appropriate for this command.</returns>
        protected virtual NavTuple GetNavTuple() => null;
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
            UseHandleExecutionException = true;
            RethrowExecutionException = false;
        }

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        protected override void HandleExecutionException(Exception e)
        {
            DialogService.ShowDialogAsync("Operation failed", e.Message, "Oh well");
            // TODO log exception?
        }

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
            await NavigateAsync(navTuple);
        }

        /// <summary>
        ///     Perform the navigation step or steps. Default implementation directly navigates to the page.
        /// </summary>
        /// <param name="navTuple"></param>
        /// <returns></returns>
        protected virtual async Task NavigateAsync(NavTuple navTuple)
        {
            await NavigationService.NavigateAsync(navTuple.Page, navTuple.ViewModel);
        }


        /// <summary>
        ///     Creates error message shown when <see cref="GetNavTuple" /> returns null.
        /// </summary>
        /// <param name="parameter">Parameter passed to command.</param>
        /// <returns>Error message</returns>
        protected virtual string GetErrorString(T parameter)
        {
            return $"Currently there is no implementation to open '{parameter}'";
        }

        /// <summary>
        ///     Create pair of navigation arguments for given parameter. May return null to show navigation error.
        /// </summary>
        /// <param name="parameter">Parameter passed to command.</param>
        /// <returns>Nav tuple appropriate for given parameter.</returns>
        protected virtual NavTuple GetNavTuple(T parameter) => null;
    }
}
