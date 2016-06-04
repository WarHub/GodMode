// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using DataAccess.Commands;
    using Demo;
    using Editor.ViewModels;
    using Editor.Views;
    using GodMode.Commands;
    using Model;
    using Mvvm;
    using StorageExplorer.Commands;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(NavigateCommandFactory navigateCommandFactory,
            Func<ICatalogue, CatalogueViewModel> catalogueVmFactory,
            OpenRemoteDataSourcesCommand openRemoteDataIndexCommand,
            OpenStorageHomeCommand openStorageHomeCommand, OpenDataIndexCommand openDataIndexCommand)
        {
            OpenCatalogueCommand = navigateCommandFactory(() =>
            {
                var vm = catalogueVmFactory(DemoLoader.Catalogue);
                return new NavTuple(new CataloguePage(), vm);
            });
            OpenRemoteDataIndexCommand = openRemoteDataIndexCommand;
            OpenStorageHomeCommand = openStorageHomeCommand;
            OpenDataIndexCommand = openDataIndexCommand;
        }

        public string MainText { get; } = "Welcome in GodMode! This is Xamarin.Forms application.";

        public ICommand OpenCatalogueCommand { get; }

        public OpenDataIndexCommand OpenDataIndexCommand { get; }

        public OpenRemoteDataSourcesCommand OpenRemoteDataIndexCommand { get; }

        public OpenStorageHomeCommand OpenStorageHomeCommand { get; }
    }
}
