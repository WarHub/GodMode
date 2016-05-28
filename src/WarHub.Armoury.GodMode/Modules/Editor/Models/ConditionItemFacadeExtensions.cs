// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using Model;
    using Mvvm.Commands;

    public static class ConditionItemFacadeExtensions
    {
        public static ConditionItemFacade ToFacade(this ICatalogueCondition condition,
            ICommand<ConditionItemFacade> removeCommand)
        {
            return new ConditionItemFacade(condition, removeCommand);
        }

        public static ConditionItemFacade ToFacade(this ICatalogueConditionGroup group,
            ICommand<ConditionItemFacade> removeCommand)
        {
            return new ConditionItemFacade(group, removeCommand);
        }
    }
}
