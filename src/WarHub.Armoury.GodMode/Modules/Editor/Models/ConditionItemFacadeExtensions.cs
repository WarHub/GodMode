// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using Model;

    public static class ConditionItemFacadeExtensions
    {
        public static ConditionItemFacade ToFacade(this ICatalogueCondition condition)
        {
            return new ConditionItemFacade(condition);
        }

        public static ConditionItemFacade ToFacade(this ICatalogueConditionGroup group)
        {
            return new ConditionItemFacade(group);
        }
    }
}
