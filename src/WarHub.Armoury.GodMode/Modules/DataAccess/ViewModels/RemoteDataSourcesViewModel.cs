// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Commands;
    using Model;
    using Model.DataAccess;
    using Mvvm;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoteDataSourcesViewModel : ViewModelBase
    {
        public RemoteDataSourcesViewModel(IRemoteDataService remoteDataService,
            RemoveDataSourceCommand removeDataSourceCommand,
            BeginAddRemoteDataSourceCommand beginAddRemoteDataSourceCommand,
            OpenRemoteDataSourceIndexCommand openRemoteDataSourceIndexCommand)
        {
            RemoteDataService = remoteDataService;
            RemoveDataSourceCommand = removeDataSourceCommand;
            BeginAddRemoteDataSourceCommand = beginAddRemoteDataSourceCommand;
            OpenRemoteDataSourceIndexCommand = openRemoteDataSourceIndexCommand;
        }

        public BeginAddRemoteDataSourceCommand BeginAddRemoteDataSourceCommand { get; }

        public IObservableReadonlySet<RemoteDataSourceInfo> DataSourceInfos => RemoteDataService.SourceInfos;

        public OpenRemoteDataSourceIndexCommand OpenRemoteDataSourceIndexCommand { get; }

        public RemoveDataSourceCommand RemoveDataSourceCommand { get; }

        private IRemoteDataService RemoteDataService { get; }
    }
}
