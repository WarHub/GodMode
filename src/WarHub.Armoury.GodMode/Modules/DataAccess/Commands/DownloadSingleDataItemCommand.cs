// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BattleScribe.Files;
    using Model.Repo;
    using Mvvm.Commands;
    using ViewModels;

    public class DownloadSingleDataItemCommand : ProgressingAsyncCommandBase<RemoteDataUpdateInfoViewModel>
    {
        public DownloadSingleDataItemCommand(IRepoStorageService repoStorageService)
        {
            RepoStorageService = repoStorageService;
        }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataUpdateInfoViewModel parameter)
        {
            var remoteDataInfo = parameter.RemoteDataInfo;
            var indexUri = parameter.IndexUri;
            OperationTitle = $"Downloading {remoteDataInfo.Name}";
            var uri = new Uri(indexUri, remoteDataInfo.IndexPathSuffix);
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
        }
    }
}
