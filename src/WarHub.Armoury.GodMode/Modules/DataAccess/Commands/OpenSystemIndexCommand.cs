// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using AppServices;
    using GodMode.Commands;
    using Model.Repo;
    using ViewModels;
    using Views;

    public class OpenSystemIndexCommand : NavigateCommandBase<ISystemIndex>
    {
        public OpenSystemIndexCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, SystemIndexVmFactory systemIndexVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            SystemIndexVmFactory = systemIndexVmFactory;
        }

        private SystemIndexVmFactory SystemIndexVmFactory { get; }

        protected override NavTuple GetNavTuple(ISystemIndex parameter)
        {
            var vm = SystemIndexVmFactory(parameter);
            return new NavTuple(new SystemIndexPage(), vm);
        }

        protected override bool CanExecuteCore(ISystemIndex parameter) => parameter != null;
    }
}
