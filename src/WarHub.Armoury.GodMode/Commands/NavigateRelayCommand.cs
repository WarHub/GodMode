// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AppServices;

    public delegate NavTuple NavTupleFactory();

    public delegate NavTuple NavTupleFactory<in T>(T parameter);

    public delegate NavigateRelayCommand<T> NavigateWithParamCommandFactory<T>(
        NavTupleFactory<T> navTupleFactory, Func<NavTuple, INavigationService, Task> navigateAsyncFunc = null,
        Func<T, bool> canExecuteFunc = null);

    public delegate NavigateRelayCommand NavigateCommandFactory(
        NavTupleFactory navTupleFactory, Func<NavTuple, INavigationService, Task> navigateAsyncFunc = null,
        Func<bool> canExecuteFunc = null);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class NavigateRelayCommand<T> : NavigateCommandBase<T>
    {
        public NavigateRelayCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, NavTupleFactory<T> navTupleFactory,
            Func<NavTuple, INavigationService, Task> navigateAsyncFunc = null, Func<T, bool> canExecuteFunc = null)
            : base(dependencyAggregate, navigationService)
        {
            if (navTupleFactory == null)
                throw new ArgumentNullException(nameof(navTupleFactory));
            CanExecuteFunc = canExecuteFunc;
            NavTupleFactory = navTupleFactory;
            NavigateAsyncFunc = navigateAsyncFunc;
        }

        private Func<T, bool> CanExecuteFunc { get; }

        private Func<NavTuple, INavigationService, Task> NavigateAsyncFunc { get; }

        private NavTupleFactory<T> NavTupleFactory { get; }

        protected override async Task NavigateAsync(NavTuple navTuple)
        {
            if (NavigateAsyncFunc != null)
            {
                await NavigateAsyncFunc(navTuple, NavigationService);
            }
            else
            {
                await base.NavigateAsync(navTuple);
            }
        }

        protected override NavTuple GetNavTuple(T parameter)
        {
            return NavTupleFactory(parameter);
        }

        protected override bool CanExecuteCore(T parameter)
        {
            return CanExecuteFunc?.Invoke(parameter) ?? true;
        }
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class NavigateRelayCommand : NavigateCommandBase
    {
        public NavigateRelayCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, NavTupleFactory navTupleFactory,
            Func<NavTuple, INavigationService, Task> navigateAsyncFunc = null, Func<bool> canExecuteFunc = null)
            : base(dependencyAggregate, navigationService)
        {
            if (navTupleFactory == null)
                throw new ArgumentNullException(nameof(navTupleFactory));
            CanExecuteFunc = canExecuteFunc;
            NavigateAsyncFunc = navigateAsyncFunc;
            NavTupleFactory = navTupleFactory;
        }

        private Func<bool> CanExecuteFunc { get; }

        private Func<NavTuple, INavigationService, Task> NavigateAsyncFunc { get; }

        private NavTupleFactory NavTupleFactory { get; }

        protected override async Task NavigateAsync(NavTuple navTuple)
        {
            if (NavigateAsyncFunc != null)
            {
                await NavigateAsyncFunc(navTuple, NavigationService);
            }
            else
            {
                await base.NavigateAsync(navTuple);
            }
        }

        protected override NavTuple GetNavTuple()
        {
            return NavTupleFactory();
        }

        protected override bool CanExecuteCore()
        {
            return CanExecuteFunc?.Invoke() ?? true;
        }
    }
}
