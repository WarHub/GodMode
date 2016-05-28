// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Commands;
    using Model;
    using Model.DataAccess;
    using Model.Repo;
    using Mvvm;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoteDataSourcesViewModel : ViewModelBase
    {
        public RemoteDataSourcesViewModel(IRemoteSourceIndexService remoteSourceIndexService,
            RemoveDataSourceCommand removeDataSourceCommand,
            BeginAddRemoteDataSourceCommand beginAddRemoteDataSourceCommand,
            OpenRemoteDataSourceIndexCommand openRemoteDataSourceIndexCommand)
        {
            RemoteSourceIndexService = remoteSourceIndexService;
            RemoveDataSourceCommand = removeDataSourceCommand;
            BeginAddRemoteDataSourceCommand = beginAddRemoteDataSourceCommand;
            OpenRemoteDataSourceIndexCommand = openRemoteDataSourceIndexCommand;
        }

        public BeginAddRemoteDataSourceCommand BeginAddRemoteDataSourceCommand { get; }

        public IObservableReadonlySet<RemoteSource> DataSourceInfos => RemoteSourceIndexService.SourceInfos;

        public OpenRemoteDataSourceIndexCommand OpenRemoteDataSourceIndexCommand { get; }

        public RemoveDataSourceCommand RemoveDataSourceCommand { get; }

        private IRemoteSourceIndexService RemoteSourceIndexService { get; }
    }
}
