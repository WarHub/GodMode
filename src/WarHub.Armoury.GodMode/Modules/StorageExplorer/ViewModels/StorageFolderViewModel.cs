// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Input;
    using Bindables;
    using Commands;
    using Mvvm;
    using PCLStorage;

    public delegate StorageFolderViewModel StorageFolderVmFactory(IFolder folder);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class StorageFolderViewModel : ViewModelBase
    {
        private const string FileGroupName = "Files";
        private const string FolderGroupName = "Folders";

        private IBindableGrouping<IFile> _files = Enumerable.Empty<IFile>()
            .ToList()
            .ToBindableGrouping(FileGroupName, FileGroupName);

        private IBindableGrouping<IFolder> _folders =
            Enumerable.Empty<IFolder>().ToList().ToBindableGrouping(FolderGroupName, FolderGroupName);

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public StorageFolderViewModel(IFolder folder, DeleteFileCommand deleteFileCommand,
            DeleteFolderCommand deleteFolderCommand, OpenStorageItemCommand openStorageItemCommand)
        {
            Folder = folder;
            OpenStorageItemCommand = openStorageItemCommand;
            DeleteFileCommand = deleteFileCommand.WrapWith(postExecuteAction: _ => UpdateFilesAsync());
            DeleteFolderCommand = deleteFolderCommand.WrapWith(postExecuteAction: _ => UpdateFoldersAsync());
            UpdateItems();
        }

        public ICommand DeleteFileCommand { get; }

        public ICommand DeleteFolderCommand { get; }

        public IBindableGrouping<IFile> Files
        {
            get { return _files; }
            private set
            {
                if (Set(ref _files, value))
                {
                    RaisePropertyChanged(nameof(Items));
                }
            }
        }

        public IFolder Folder { get; }

        public IBindableGrouping<IFolder> Folders
        {
            get { return _folders; }
            private set
            {
                if (Set(ref _folders, value))
                {
                    RaisePropertyChanged(nameof(Items));
                }
            }
        }

        public IEnumerable<IBindableGrouping<object>> Items
        {
            get
            {
                yield return Folders;
                yield return Files;
            }
        }

        public OpenStorageItemCommand OpenStorageItemCommand { get; }

        private void UpdateItems()
        {
            UpdateFoldersAsync();
            UpdateFilesAsync();
        }

        private async void UpdateFilesAsync()
        {
            var files = await Folder.GetFilesAsync();
            Files = files.ToBindableGrouping(FileGroupName, FileGroupName);
        }

        private async void UpdateFoldersAsync()
        {
            var folders = await Folder.GetFoldersAsync();
            Folders = folders.ToBindableGrouping(FolderGroupName, FolderGroupName);
        }
    }
}
