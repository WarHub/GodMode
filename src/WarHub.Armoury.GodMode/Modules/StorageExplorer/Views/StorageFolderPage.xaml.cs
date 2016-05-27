// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Views
{
    using Xamarin.Forms;

    public partial class StorageFolderPage : ContentPage
    {
        public StorageFolderPage()
        {
            InitializeComponent();
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ListView.Filter(e.NewTextValue);
        }
    }
}
