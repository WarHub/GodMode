// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class OpenStorageHomeCommand : NavigateCommandBase
    {
        public OpenStorageHomeCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<StorageHomeViewModel> storageHomeViewModelFactory)
            : base(dependencyAggregate, navigationService)
        {
            StorageHomeViewModelFactory = storageHomeViewModelFactory;
        }

        private Func<StorageHomeViewModel> StorageHomeViewModelFactory { get; }

        protected override NavTuple GetNavTuple()
        {
            var vm = StorageHomeViewModelFactory();
            return new NavTuple(new StorageHomePage(), vm);
        }
    }
}
