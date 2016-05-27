// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Controls
{
    using GodMode.Controls;
    using PCLStorage;

    public class GroupingStorageItemListView : GroupingFilteringListViewBase<object>
    {
        protected override string SelectKey(object item)
        {
            var folder = item as IFolder;
            if (folder != null)
            {
                return folder.Name;
            }
            var file = item as IFile;
            return file != null ? file.Name : string.Empty;
        }
    }
}
