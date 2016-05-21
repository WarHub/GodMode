// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.DataAccess;
    using Model.Repo;
    using Mvvm.Commands;

    public class DownloadDataSourceCommand : ProgressingAsyncCommandBase<RemoteDataSourceInfo>
    {
        public DownloadDataSourceCommand(IRemoteDataService remoteDataService, IRepoStorageService repoStorageService)
        {
            RemoteDataService = remoteDataService;
            RepoStorageService = repoStorageService;
        }

        private IRemoteDataService RemoteDataService { get; }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataSourceInfo parameter)
        {
            var index = await RemoteDataService.DownloadIndexAsync(parameter);
            var i = 0;
            foreach (var remoteDataInfo in index.RemoteDataInfos)
            {
                OperationTitle = $"Downloading {remoteDataInfo.Name}";
                var uri = new Uri(index.IndexUri, remoteDataInfo.IndexPathSuffix);
                using (var client = new HttpClient())
                using (var stream = await client.GetStreamAsync(uri))
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    switch (remoteDataInfo.DataType)
                    {
                        case RemoteDataType.Catalogue:
                            var catInfo = CatalogueInfo.CreateFromStream(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            using (var outStream = await RepoStorageService.GetCatalogueOutputStreamAsync(catInfo))
                            {
                                await memoryStream.CopyToAsync(outStream);
                            }
                            break;
                        case RemoteDataType.GameSystem:
                            var gstInfo = GameSystemInfo.CreateFromStream(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            using (var outStream = await RepoStorageService.GetGameSystemOutputStreamAsync(gstInfo))
                            {
                                await memoryStream.CopyToAsync(outStream);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                ProgressPercent = (++i/index.RemoteDataInfos.Count)*100;
            }
        }

        protected override bool CanExecuteCore(RemoteDataSourceInfo parameter) => parameter != null;
    }
}
