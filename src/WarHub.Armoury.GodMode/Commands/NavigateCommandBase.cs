// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AppServices;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
    public abstract class NavigateCommandBase : AppAsyncCommandBase
    {
        protected NavigateCommandBase(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService) : base(dependencyAggregate)
        {
            NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; }

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
            await NavigationService.NavigateAsync(navTuple);
        }

        /// <summary>
        ///     Creates error message shown when <see cref="GetNavTuple" /> returns null.
        /// </summary>
        /// <returns>Error message</returns>
        protected virtual string GetErrorString()
        {
            return "Currently there is no implementation to open requested view.";
        }

        /// <summary>
        ///     Create pair of navigation arguments for given parameter. May return null to show navigation error.
        /// </summary>
        /// <returns>Nav tuple appropriate for this command.</returns>
        protected virtual NavTuple GetNavTuple() => null;
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class NavigateCommandBase<T> : AppAsyncCommandBase<T>
    {
        protected NavigateCommandBase(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService) : base(dependencyAggregate)
        {
            NavigationService = navigationService;
        }

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
            await NavigateAsync(navTuple);
        }

        /// <summary>
        ///     Perform the navigation step or steps. Default implementation directly navigates to the page.
        /// </summary>
        /// <param name="navTuple"></param>
        /// <returns></returns>
        protected virtual async Task NavigateAsync(NavTuple navTuple)
        {
            await NavigationService.NavigateAsync(navTuple);
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
