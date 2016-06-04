// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    public class OpenDataIndexCommand : NavigateCommandBase
    {
        public OpenDataIndexCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<DataIndexViewModel> dataIndexVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            DataIndexVmFactory = dataIndexVmFactory;
        }

        private Func<DataIndexViewModel> DataIndexVmFactory { get; }

        protected override NavTuple GetNavTuple()
        {
            var vm = DataIndexVmFactory();
            return new NavTuple(new DataIndexPage(), vm);
        }
    }
}
