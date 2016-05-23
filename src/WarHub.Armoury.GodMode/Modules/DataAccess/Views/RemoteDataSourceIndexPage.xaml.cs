// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Views
{
    using Xamarin.Forms;

    public partial class RemoteDataSourceIndexPage : ContentPage
    {
        public RemoteDataSourceIndexPage()
        {
            InitializeComponent();
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ItemsListView.Filter(e.NewTextValue);
        }
    }
}
