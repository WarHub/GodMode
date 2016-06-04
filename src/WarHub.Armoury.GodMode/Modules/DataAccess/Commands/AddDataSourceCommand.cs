// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using Model.DataAccess;
    using Model.Repo;
    using ViewModels;

    public class AddDataSourceCommand : NavigateCommandBase<AddRemoteDataSourceViewModel>
    {
        public AddDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, IRemoteSourceIndexService remoteSourceIndexService)
            : base(dependencyAggregate, navigationService)
        {
            RemoteSourceIndexService = remoteSourceIndexService;
        }

        private IRemoteSourceIndexService RemoteSourceIndexService { get; }

        protected override bool CanExecuteCore(AddRemoteDataSourceViewModel parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            parameter.PropertyChanged -= Parameter_PropertyChanged;
            parameter.PropertyChanged += Parameter_PropertyChanged;
            return IsUrlValid(parameter);
        }

        private void Parameter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == null || e.PropertyName == nameof(AddRemoteDataSourceViewModel.Url))
            {
                RaiseCanExecuteChanged();
            }
        }

        protected override async Task ExecuteCoreAsync(AddRemoteDataSourceViewModel parameter)
        {
            var remoteSourceInfo = new RemoteSource(parameter.Name, parameter.Url);
            RemoteSourceIndexService.AddSource(remoteSourceInfo);
            await NavigationService.PopAsync();
        }

        private static bool IsUrlValid(AddRemoteDataSourceViewModel parameter)
        {
            Uri uri;
            return Uri.TryCreate(parameter.Url, UriKind.Absolute, out uri);
        }
    }
}
