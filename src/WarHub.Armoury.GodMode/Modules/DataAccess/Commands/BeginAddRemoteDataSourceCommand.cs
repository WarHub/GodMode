// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class BeginAddRemoteDataSourceCommand : NavigateCommandBase
    {
        public BeginAddRemoteDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<AddRemoteDataSourceViewModel> addRemoteDataSourceVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            AddRemoteDataSourceVmFactory = addRemoteDataSourceVmFactory;
        }

        private Func<AddRemoteDataSourceViewModel> AddRemoteDataSourceVmFactory { get; }

        protected override NavTuple GetNavTuple()
        {
            return new NavTuple(new AddRemoteDataSourcePage(), AddRemoteDataSourceVmFactory());
        }
    }
}
