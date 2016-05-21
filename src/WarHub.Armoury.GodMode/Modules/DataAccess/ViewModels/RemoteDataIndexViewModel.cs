// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System.Windows.Input;
    using Commands;
    using Model;
    using Model.DataAccess;
    using Mvvm;

    public class RemoteDataIndexViewModel : ViewModelBase
    {
        public RemoteDataIndexViewModel(IRemoteDataService remoteDataService,
            DownloadDataSourceCommand downloadDataSourceCommand, RemoveDataSourceCommand removeDataSourceCommand,
            BeginAddRemoteDataSourceCommand beginAddRemoteDataSourceCommand)
        {
            RemoteDataService = remoteDataService;
            DownloadDataSourceCommand = downloadDataSourceCommand;
            RemoveDataSourceCommand = removeDataSourceCommand;
            BeginAddRemoteDataSourceCommand = beginAddRemoteDataSourceCommand;
        }

        public ICommand BeginAddRemoteDataSourceCommand { get; }


        public IObservableReadonlySet<RemoteDataSourceInfo> DataSourceInfos => RemoteDataService.SourceInfos;

        public ICommand DownloadDataSourceCommand { get; }

        public ICommand RemoveDataSourceCommand { get; }

        private IRemoteDataService RemoteDataService { get; }
    }
}
