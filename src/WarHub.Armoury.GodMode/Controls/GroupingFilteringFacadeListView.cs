// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Controls
{
    using ModelFacades;

    public class GroupingFilteringFacadeListView : GroupingFilteringListViewBase<IModelFacade>
    {
        public GroupingFilteringFacadeListView()
        {
            ItemTemplate = FacadeCellTemplateFactory.Create();
        }

        public override string SelectKey(IModelFacade item) => item.Name;
    }
}
