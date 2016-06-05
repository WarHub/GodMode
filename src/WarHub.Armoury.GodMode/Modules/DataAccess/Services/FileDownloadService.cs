// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Model.DataAccess;
    using PCLStorage;

    public class FileDownloadService : IFileDownloadService
    {
        public FileDownloadService(IFileSystem fileSystem, TempFolderFactory tempFolderFactory)
        {
            FileSystem = fileSystem;
            TempFolderFactory = tempFolderFactory;
        }

        private IFileSystem FileSystem { get; }

        private TempFolderFactory TempFolderFactory { get; }

        public async Task<IFile> DownloadFileAsync(Uri uri, IFolder folder)
        {
            var file = await folder.CreateFileAsync(uri.Segments.Last(), CreationCollisionOption.GenerateUniqueName);
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(uri))
            using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                await stream.CopyToAsync(fileStream);
            }
            return file;
        }

        public async Task<ITempFolder> CreateTempFolderAsync()
        {
            var folderName = $"Temp_{Guid.NewGuid()}";
            var folder =
                await FileSystem.LocalStorage.CreateFolderAsync(folderName, CreationCollisionOption.GenerateUniqueName);
            var tempFolder = TempFolderFactory(folder);
            return tempFolder;
        }
    }

    public delegate ITempFolder TempFolderFactory(IFolder baseFolder);

    public sealed class TempFolderService : ITempFolder
    {
        public TempFolderService(IFolder baseFolder, ILog log)
        {
            BaseFolder = baseFolder;
            Log = log;
        }

        private IFolder BaseFolder { get; }

        private bool IsDisposed { get; set; }

        private ILog Log { get; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            DisposeCore();
        }

        public Task<IFile> CreateFileAsync(string desiredName, CreationCollisionOption option,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.CreateFileAsync(desiredName, option, cancellationToken);
        }

        public Task<IFile> GetFileAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.GetFileAsync(name, cancellationToken);
        }

        public Task<IList<IFile>> GetFilesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.GetFilesAsync(cancellationToken);
        }

        public Task<IFolder> CreateFolderAsync(string desiredName, CreationCollisionOption option,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.CreateFolderAsync(desiredName, option, cancellationToken);
        }

        public Task<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.GetFolderAsync(name, cancellationToken);
        }

        public Task<IList<IFolder>> GetFoldersAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.GetFoldersAsync(cancellationToken);
        }

        public Task<ExistenceCheckResult> CheckExistsAsync(string name,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.CheckExistsAsync(name, cancellationToken);
        }

        public Task DeleteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return BaseFolder.DeleteAsync(cancellationToken);
        }

        public string Name => BaseFolder.Name;

        public string Path => BaseFolder.Path;

        private async void DisposeCore()
        {
            try
            {
                await BaseFolder.DeleteAsync();
            }
            catch (Exception e)
            {
                Log.Warn?.With("Error during dispose.", e);
            }
        }
    }
}
