// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Controls
{
    using Xamarin.Forms;

    public class FacadeListView : ListView
    {
        public FacadeListView()
        {
            ItemTemplate = FacadeCellTemplateFactory.Create();
        }
    }
}
