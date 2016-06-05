// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.BattleScribe.Files;
    using Model.Repo;
    using PCLStorage;
    using Services;
    using ViewModels;

    public class DownloadDataSourceCommand : AppAsyncCommandBase<RemoteDataSourceIndexViewModel>
    {
        public DownloadDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRepoStorageService repoStorageService, IFileDownloadService fileDownloadService)
            : base(dependencyAggregate)
        {
            RepoStorageService = repoStorageService;
            FileDownloadService = fileDownloadService;
            OperationTitle = "Ready";
            IsExecutionBlocking = true;
        }

        private IFileDownloadService FileDownloadService { get; }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataSourceIndexViewModel parameter)
        {
            var updateInfos =
                parameter.GameSystemUpdateInfoViewModels.Concat(parameter.CatalogueUpdateInfoViewModels).ToArray();
            var i = 0;
            using (var tempFolder = await FileDownloadService.CreateTempFolderAsync())
            {
                foreach (var updateInfo in updateInfos)
                {
                    await DownloadCoreAsync(tempFolder, updateInfo);
                    ProgressPercent = (int?) (100d*++i/updateInfos.Length);
                }
            }
            OperationTitle = "Ready";
        }

        private async Task DownloadCoreAsync(IFolder tempFolder, RemoteDataUpdateInfoViewModel updateInfo)
        {
            var remoteDataInfo = updateInfo.RemoteDataInfo;
            OperationTitle = $"Downloading {remoteDataInfo.Name}";
            var uri = new Uri(updateInfo.IndexUri, remoteDataInfo.IndexPathSuffix);
            var file = await FileDownloadService.DownloadFileAsync(uri, tempFolder);
            using (var fileStream = await file.OpenAsync(FileAccess.Read))
            {
                var filename = uri.Segments.Last();
                await CopyToRepoStorage(remoteDataInfo, filename, fileStream);
            }
            updateInfo.UpdateLocalInfo();
        }

        private async Task CopyToRepoStorage(RemoteDataInfo remoteDataInfo, string filename, Stream fileStream)
        {
            switch (remoteDataInfo.DataType)
            {
                case RemoteDataType.Catalogue:
                    await
                        CatalogueFile.MoveToRepoStorageAsync(fileStream, filename, RepoStorageService);
                    break;
                case RemoteDataType.GameSystem:
                    await
                        GameSystemFile.MoveToRepoStorageAsync(fileStream, filename, RepoStorageService);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unrecognized {nameof(RemoteDataType)} of {remoteDataInfo.Name} ({remoteDataInfo.IndexPathSuffix})");
            }
        }

        protected override bool CanExecuteCore(RemoteDataSourceIndexViewModel parameter) => parameter != null;
    }
}
