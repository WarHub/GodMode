// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using DataAccess.Commands;
    using Demo;
    using Editor.Commands;
    using Model.Repo;
    using Mvvm;
    using StorageExplorer.Commands;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            OpenCatalogueCommand openCatalogueCommand,
            OpenRemoteDataSourcesCommand openRemoteDataIndexCommand,
            OpenStorageHomeCommand openStorageHomeCommand)
        {
            OpenCatalogueCommand = openCatalogueCommand.SetParameter(new CatalogueInfo(DemoLoader.Catalogue));
            OpenRemoteDataIndexCommand = openRemoteDataIndexCommand;
            OpenStorageHomeCommand = openStorageHomeCommand;
        }

        public string MainText { get; } = "Welcome in GodMode! This is Xamarin.Forms application.";

        public ICommand OpenCatalogueCommand { get; }

        public OpenRemoteDataSourcesCommand OpenRemoteDataIndexCommand { get; }

        public OpenStorageHomeCommand OpenStorageHomeCommand { get; }
    }
}
