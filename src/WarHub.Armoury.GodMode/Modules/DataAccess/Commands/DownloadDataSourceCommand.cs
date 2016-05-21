// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BattleScribe.Files;
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
                {
                    switch (remoteDataInfo.DataType)
                    {
                        case RemoteDataType.Catalogue:
                            await CatalogueFile.MoveToRepoStorageAsync(stream, uri.Segments.Last(), RepoStorageService);
                            break;
                        case RemoteDataType.GameSystem:
                            await GameSystemFile.MoveToRepoStorageAsync(stream, uri.Segments.Last(), RepoStorageService);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                ProgressPercent = ++i/index.RemoteDataInfos.Count*100;
            }
        }

        protected override bool CanExecuteCore(RemoteDataSourceInfo parameter) => parameter != null;
    }
}
