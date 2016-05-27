// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.ViewModels
{
    using System.Windows.Input;
    using AppServices;
    using DataAccess.Commands;
    using Demo;
    using Model.Repo;
    using Mvvm;
    using StorageExplorer.Commands;

    public class MainViewModel : ViewModelBase
    {
        private ICommand _openCatalogueCommand;

        public MainViewModel(ICommandsAggregateService commands, OpenRemoteDataSourcesCommand openRemoteDataIndexCommand,
            OpenStorageHomeCommand openStorageHomeCommand)
        {
            Commands = commands;
            OpenRemoteDataIndexCommand = openRemoteDataIndexCommand;
            OpenStorageHomeCommand = openStorageHomeCommand;
        }

        public string MainText { get; } = "Welcome in GodMode! This is Xamarin.Forms application.";

        public ICommand OpenCatalogueCommand
            =>
                _openCatalogueCommand ??
                (_openCatalogueCommand =
                    Commands.OpenCatalogueCommand.SetParameter(new CatalogueInfo(DemoLoader.Catalogue)));

        public OpenRemoteDataSourcesCommand OpenRemoteDataIndexCommand { get; }

        public OpenStorageHomeCommand OpenStorageHomeCommand { get; }

        private ICommandsAggregateService Commands { get; }
    }
}
