// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.ViewModels
{
    using System.Windows.Input;
    using Editor.Views;
    using Mvvm;
    using Services;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        private ICommand _openCatalogueCommand;

        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;
        }

        public string MainText { get; set; } = "Welcome in GodMode! This is Xamarin.Forms application.";

        public ICommand OpenCatalogueCommand
            =>
                _openCatalogueCommand ??
                (_openCatalogueCommand = new Command(async () => { await Navigation.PushAsync(new CataloguePage()); }));

        private INavigationService Navigation { get; }
    }
}
