// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.ViewModels
{
    using System.Windows.Input;
    using AppServices;
    using DataAccess.Commands;
    using Model.Repo;
    using Mvvm;

    public class MainViewModel : ViewModelBase
    {
        private ICommand _openCatalogueCommand;

        public MainViewModel(ICommandsAggregateService commands, OpenRemoteDataIndexCommand openRemoteDataIndexCommand)
        {
            Commands = commands;
            OpenRemoteDataIndexCommand = openRemoteDataIndexCommand;
        }

        public string MainText { get; } = "Welcome in GodMode! This is Xamarin.Forms application.";

        public ICommand OpenCatalogueCommand
            => _openCatalogueCommand ?? (_openCatalogueCommand = Commands.OpenCatalogueCommand.SetParameter(new CatalogueInfo(Demo.DemoLoader.Catalogue)));

        public OpenRemoteDataIndexCommand OpenRemoteDataIndexCommand { get; }

        private ICommandsAggregateService Commands { get; }
    }
}
