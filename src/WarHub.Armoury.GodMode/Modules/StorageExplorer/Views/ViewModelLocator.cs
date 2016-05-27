// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Views
{
    using ViewModels;

    public static class ViewModelLocator
    {
        public static StorageFileDetailsViewModel StorageFileDetailsViewModel => Resolve<StorageFileDetailsViewModel>();

        public static StorageFolderViewModel StorageFolderViewModel => Resolve<StorageFolderViewModel>();

        public static StorageHomeViewModel StorageHomeViewModel => Resolve<StorageHomeViewModel>();

        private static TService Resolve<TService>() where TService : class => null;
    }
}
