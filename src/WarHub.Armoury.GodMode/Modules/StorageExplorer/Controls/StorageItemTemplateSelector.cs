// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Controls
{
    using PCLStorage;
    using Xamarin.Forms;

    public class StorageItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }

        public DataTemplate FolderTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is IFolder)
            {
                return FolderTemplate;
            }
            if (item is IFile)
            {
                return FileTemplate;
            }
            return null;
        }
    }
}
