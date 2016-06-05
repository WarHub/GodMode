// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Commands
{
    using AppServices;
    using GodMode.Commands;
    using PCLStorage;
    using ViewModels;
    using Views;

    public class OpenStorageItemCommand : NavigateCommandBase<object>
    {
        public OpenStorageItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, StorageFolderVmFactory storageFolderVmFactory,
            StorageFileDetailsVmFactory storageFileDetailsVmFactory) : base(dependencyAggregate, navigationService)
        {
            StorageFolderVmFactory = storageFolderVmFactory;
            StorageFileDetailsVmFactory = storageFileDetailsVmFactory;
        }

        private StorageFileDetailsVmFactory StorageFileDetailsVmFactory { get; }

        private StorageFolderVmFactory StorageFolderVmFactory { get; }

        protected override NavTuple GetNavTuple(object parameter)
        {
            var folder = parameter as IFolder;
            if (folder != null)
            {
                var vm = StorageFolderVmFactory(folder);
                return new NavTuple(new StorageFolderPage(), vm);
            }
            var file = parameter as IFile;
            if (file != null)
            {
                var vm = StorageFileDetailsVmFactory(file);
                return new NavTuple(new StorageFilePage(), vm);
            }
            return null;
        }

        protected override bool CanExecuteCore(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            return parameter is IFile || parameter is IFolder;
        }
    }
}
