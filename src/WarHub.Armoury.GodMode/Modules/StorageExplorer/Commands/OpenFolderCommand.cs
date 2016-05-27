// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using AppServices;
    using GodMode.Commands;
    using PCLStorage;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class OpenFolderCommand : NavigateCommandBase<IFolder>
    {
        public OpenFolderCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, StorageFolderVmFactory storageFolderVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            StorageFolderVmFactory = storageFolderVmFactory;
        }

        private StorageFolderVmFactory StorageFolderVmFactory { get; }

        protected override NavTuple GetNavTuple(IFolder parameter)
        {
            var vm = StorageFolderVmFactory(parameter);
            return new NavTuple(new StorageFolderPage(), vm);
        }

        protected override bool CanExecuteCore(IFolder parameter) => parameter != null;
    }
}
