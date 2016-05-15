// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Threading.Tasks;
    using AppServices;
    using Mvvm.Commands;

    public class NavigateRelayCommand<T> : ProgressingAsyncCommandBase<T>
    {
        public NavigateRelayCommand(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; }

        private Func<T, bool> CanExecuteFunc { get; set; }

        private Func<T, INavigationService, Task> NavigateAsync { get; set; }

        public NavigateRelayCommand<T> Configure(Func<T, INavigationService, Task> navigateAsync,
            Func<T, bool> canExecuteFunc = null)
        {
            return new NavigateRelayCommand<T>(NavigationService)
            {
                NavigateAsync = navigateAsync,
                CanExecuteFunc = canExecuteFunc
            };
        }

        protected override async Task ExecuteCoreAsync(T parameter)
        {
            var task = NavigateAsync?.Invoke(parameter, NavigationService);
            if (task != null)
            {
                await task;
            }
        }

        protected override bool CanExecuteCore(T parameter)
        {
            return CanExecuteFunc?.Invoke(parameter) ?? true;
        }
    }

    public class NavigateRelayCommand : ProgressingAsyncCommandBase
    {
        public NavigateRelayCommand(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; }

        private Func<INavigationService, Task> NavigateAsync { get; set; }

        public NavigateRelayCommand Configure(Func<INavigationService, Task> navigateAsync)
        {
            return new NavigateRelayCommand(NavigationService) {NavigateAsync = navigateAsync};
        }

        public NavigateRelayCommand<T> Configure<T>(Func<T, INavigationService, Task> navigateAsync,
            Func<T, bool> canExecuteFunc = null)
        {
            return new NavigateRelayCommand<T>(NavigationService).Configure(navigateAsync, canExecuteFunc);
        }

        protected override async Task ExecuteCoreAsync()
        {
            var task = NavigateAsync?.Invoke(NavigationService);
            if (task != null)
            {
                await task;
            }
        }
    }
}
