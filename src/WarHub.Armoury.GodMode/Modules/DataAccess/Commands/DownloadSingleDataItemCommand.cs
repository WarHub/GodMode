// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.BattleScribe.Files;
    using Model.Repo;
    using PCLStorage;
    using Services;
    using ViewModels;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DownloadSingleDataItemCommand : AppAsyncCommandBase<RemoteDataUpdateInfoViewModel>
    {
        public DownloadSingleDataItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRepoStorageService repoStorageService, IFileDownloadService fileDownloadService)
            : base(dependencyAggregate)
        {
            RepoStorageService = repoStorageService;
            FileDownloadService = fileDownloadService;
        }

        private IFileDownloadService FileDownloadService { get; }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataUpdateInfoViewModel parameter)
        {
            var remoteDataInfo = parameter.RemoteDataInfo;
            var indexUri = parameter.IndexUri;
            OperationTitle = $"Downloading {parameter.DataName}";
            var uri = new Uri(indexUri, remoteDataInfo.IndexPathSuffix);
            using (var tempFolder = await FileDownloadService.CreateTempFolderAsync())
            {
                var file = await FileDownloadService.DownloadFileAsync(uri, tempFolder);
                using (var fileStream = await file.OpenAsync(FileAccess.Read))
                {
                    var filename = uri.Segments.Last();
                    await CopyToRepoStorage(remoteDataInfo, filename, fileStream);
                }
            }
            parameter.UpdateLocalInfo();
        }

        private async Task CopyToRepoStorage(RemoteDataInfo remoteDataInfo, string fileName, Stream fileStream)
        {
            switch (remoteDataInfo.DataType)
            {
                case RemoteDataType.Catalogue:
                    await
                        CatalogueFile.MoveToRepoStorageAsync(fileStream, fileName, RepoStorageService);
                    break;
                case RemoteDataType.GameSystem:
                    await
                        GameSystemFile.MoveToRepoStorageAsync(fileStream, fileName, RepoStorageService);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unrecognized {nameof(RemoteDataType)} of {remoteDataInfo.Name} ({remoteDataInfo.IndexPathSuffix})");
            }
        }

        protected override bool CanExecuteCore(RemoteDataUpdateInfoViewModel parameter) => parameter != null;
    }
}
