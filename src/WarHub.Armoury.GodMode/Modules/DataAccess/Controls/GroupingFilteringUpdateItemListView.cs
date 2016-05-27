// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Controls
{
    using GodMode.Controls;
    using ViewModels;

    public class GroupingFilteringUpdateItemListView : GroupingFilteringListViewBase<RemoteDataUpdateInfoViewModel>
    {
        protected override string SelectKey(RemoteDataUpdateInfoViewModel item)
        {
            return item.RemoteDataInfo.Name;
        }
    }
}
