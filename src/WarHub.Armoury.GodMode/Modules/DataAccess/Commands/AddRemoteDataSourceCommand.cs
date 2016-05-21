using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using AppServices;
    using GodMode.Commands;
    using Model.DataAccess;
    using Mvvm.Commands;
    using Xamarin.Forms;

    public class AddRemoteDataSourceCommand : NavigateCommandBase
    {
        public AddRemoteDataSourceCommand(IDialogService dialogService, INavigationService navigationService, IRemoteDataService remoteDataService) : base(dialogService, navigationService)
        {
            RemoteDataService = remoteDataService;
        }

        private IRemoteDataService RemoteDataService { get; }
        
        protected override async Task ExecuteCoreAsync()
        {
            await Task.Delay(100);
            // TODO ask for address and name - navigate to Page with Form?
        }
    }
}
