// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Mvvm;
    using PCLStorage;

    public delegate StorageFileDetailsViewModel StorageFileDetailsVmFactory(IFile file);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class StorageFileDetailsViewModel : ViewModelBase
    {
        private string _contents;

        public StorageFileDetailsViewModel(IFile file)
        {
            File = file;
            ReadContentsAsync();
        }

        public string Contents
        {
            get { return _contents; }
            set
            {
                if (Set(ref _contents, value))
                {
                    WriteContentsAsync();
                }
            }
        }

        public IFile File { get; }

        private async void WriteContentsAsync()
        {
            await File.WriteAllTextAsync(Contents);
        }

        private async void ReadContentsAsync()
        {
            Contents = await File.ReadAllTextAsync();
        }
    }
}
