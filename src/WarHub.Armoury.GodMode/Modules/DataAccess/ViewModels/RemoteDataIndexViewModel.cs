// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using Commands;
    using Model;
    using Model.DataAccess;
    using Mvvm;

    public class RemoteDataIndexViewModel : ViewModelBase
    {
        public RemoteDataIndexViewModel(IRemoteDataService remoteDataService, AddRemoteDataSourceCommand addRemoteDataSourceCommand, DownloadDataSourceCommand downloadDataSourceCommand)
        {
            RemoteDataService = remoteDataService;
            AddRemoteDataSourceCommand = addRemoteDataSourceCommand;
            DownloadDataSourceCommand = downloadDataSourceCommand;
        }

        public IObservableReadonlySet<RemoteDataSourceInfo> DataSourceInfos => RemoteDataService.SourceInfos;

        private IRemoteDataService RemoteDataService { get; }

        public AddRemoteDataSourceCommand AddRemoteDataSourceCommand { get; }

        public DownloadDataSourceCommand DownloadDataSourceCommand { get; }
    }
}
