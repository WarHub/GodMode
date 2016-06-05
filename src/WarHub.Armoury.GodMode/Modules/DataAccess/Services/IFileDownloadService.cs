// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Services
{
    using System;
    using System.Threading.Tasks;
    using PCLStorage;

    public interface IFileDownloadService
    {
        /// <summary>
        ///     Downloads file from uri into given folder.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task<IFile> DownloadFileAsync(Uri uri, IFolder folder);

        Task<ITempFolder> CreateTempFolderAsync();
    }

    public interface ITempFolder : IDisposable, IFolder
    {
    }
}
