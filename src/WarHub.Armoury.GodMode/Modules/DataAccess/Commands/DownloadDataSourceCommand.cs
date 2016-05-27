// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model.BattleScribe.Files;
    using Model.DataAccess;
    using Model.Repo;
    using ViewModels;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DownloadDataSourceCommand : AppAsyncCommandBase<RemoteDataSourceIndexViewModel>
    {
        public DownloadDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRemoteDataService remoteDataService, IRepoStorageService repoStorageService) : base(dependencyAggregate)
        {
            RemoteDataService = remoteDataService;
            RepoStorageService = repoStorageService;
            OperationTitle = "Ready";
            IsExecutionBlocking = true;
        }

        private IRemoteDataService RemoteDataService { get; }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataSourceIndexViewModel parameter)
        {
            var updateInfos =
                parameter.GameSystemUpdateInfoViewModels.Concat(parameter.CatalogueUpdateInfoViewModels).ToArray();
            var i = 0;
            foreach (var updateInfo in updateInfos)
            {
                var remoteDataInfo = updateInfo.RemoteDataInfo;
                OperationTitle = $"Downloading {remoteDataInfo.Name}";
                var uri = new Uri(updateInfo.IndexUri, remoteDataInfo.IndexPathSuffix);
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
                updateInfo.UpdateLocalInfo();
                ProgressPercent = (int?) (100d*++i/updateInfos.Length);
            }
            OperationTitle = "Ready";
        }

        protected override bool CanExecuteCore(RemoteDataSourceIndexViewModel parameter) => parameter != null;
    }
}
