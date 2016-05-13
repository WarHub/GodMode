// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Controls
{
    using Modules.Editor.Models;
    using Xamarin.Forms;

    public static class FacadeCellTemplateFactory
    {
        public static DataTemplate Create()
        {
            var cellTemplate = new DataTemplate(CreateCell);
            return cellTemplate;
        }

        private static TextCell CreateCell()
        {
            var cell = new TextCell();
            cell.SetBinding(TextCell.TextProperty, nameof(IModelFacade.Name));
            cell.SetBinding(TextCell.DetailProperty, nameof(IModelFacade.Detail));
            var removeMenuItem = new MenuItem {Text = "Remove", IsDestructive = true};
            removeMenuItem.SetBinding(MenuItem.CommandProperty, nameof(IModelFacade.RemoveCommand));
            cell.ContextActions.Add(removeMenuItem);
            return cell;
        }
    }
}
