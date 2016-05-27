// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using Model.DataAccess;
    using ViewModels;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class AddDataSourceCommand : NavigateCommandBase<AddRemoteDataSourceViewModel>
    {
        public AddDataSourceCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, IRemoteDataService remoteDataService)
            : base(dependencyAggregate, navigationService)
        {
            RemoteDataService = remoteDataService;
        }

        private IRemoteDataService RemoteDataService { get; }

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
            var remoteSourceInfo = new RemoteDataSourceInfo(parameter.Name, parameter.Url);
            RemoteDataService.AddSource(remoteSourceInfo);
            await NavigationService.PopAsync();
        }

        private static bool IsUrlValid(AddRemoteDataSourceViewModel parameter)
        {
            Uri uri;
            return Uri.TryCreate(parameter.Url, UriKind.Absolute, out uri);
        }
    }
}
