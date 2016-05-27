// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Commands;
    using Mvvm;
    using PCLStorage;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public class StorageHomeViewModel : ViewModelBase
    {
        public StorageHomeViewModel(OpenFolderCommand openFolderCommand)
        {
            OpenFolderCommand = openFolderCommand;
        }

        public IFolder LocalFolder => FileSystem.Current.LocalStorage;

        public OpenFolderCommand OpenFolderCommand { get; }

        public IFolder RoamingFolder => FileSystem.Current.RoamingStorage;
    }
}
