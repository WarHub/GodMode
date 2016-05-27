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
    public class OpenRemoteDataSourcesCommand : NavigateCommandBase
    {
        public OpenRemoteDataSourcesCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<RemoteDataSourcesViewModel> remoteDataIndexVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            RemoteDataIndexVmFactory = remoteDataIndexVmFactory;
        }

        private Func<RemoteDataSourcesViewModel> RemoteDataIndexVmFactory { get; }

        protected override NavTuple GetNavTuple()
        {
            var vm = RemoteDataIndexVmFactory();
            return new NavTuple(new RemoteDataSourcesPage(), vm);
        }
    }
}
