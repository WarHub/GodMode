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
    using Model.Repo;
    using ViewModels;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DownloadSingleDataItemCommand : AppAsyncCommandBase<RemoteDataUpdateInfoViewModel>
    {
        public DownloadSingleDataItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IRepoStorageService repoStorageService) : base(dependencyAggregate)
        {
            RepoStorageService = repoStorageService;
        }

        private IRepoStorageService RepoStorageService { get; }

        protected override async Task ExecuteCoreAsync(RemoteDataUpdateInfoViewModel parameter)
        {
            var remoteDataInfo = parameter.RemoteDataInfo;
            var indexUri = parameter.IndexUri;
            OperationTitle = $"Downloading {parameter.DataName}";
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
            parameter.UpdateLocalInfo();
        }

        protected override bool CanExecuteCore(RemoteDataUpdateInfoViewModel parameter) => parameter != null;
    }
}
