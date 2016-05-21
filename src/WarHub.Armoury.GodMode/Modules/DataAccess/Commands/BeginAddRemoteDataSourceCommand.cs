// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using AppServices;
    using GodMode.Commands;
    using ViewModels;
    using Views;

    public class BeginAddRemoteDataSourceCommand : NavigateCommandBase
    {
        public BeginAddRemoteDataSourceCommand(IDialogService dialogService, INavigationService navigationService,
            Func<AddRemoteDataSourceViewModel> addRemoteDataSourceVmFactory) : base(dialogService, navigationService)
        {
            AddRemoteDataSourceVmFactory = addRemoteDataSourceVmFactory;
        }

        public Func<AddRemoteDataSourceViewModel> AddRemoteDataSourceVmFactory { get; set; }

        protected override NavTuple GetNavTuple()
        {
            return new NavTuple(new AddRemoteDataSourcePage(), AddRemoteDataSourceVmFactory());
        }
    }
}
