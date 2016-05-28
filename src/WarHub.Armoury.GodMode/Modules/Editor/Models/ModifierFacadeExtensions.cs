// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using Model;
    using Mvvm.Commands;

    public static class ModifierFacadeExtensions
    {
        public static ModifierFacade ToFacade(this IEntryModifier modifier, ICommand<ModifierFacade> removeCommand)
        {
            return new ModifierFacade(modifier, removeCommand);
        }

        public static ModifierFacade ToFacade(this IGroupModifier modifier, ICommand<ModifierFacade> removeCommand)
        {
            return new ModifierFacade(modifier, removeCommand);
        }

        public static ModifierFacade ToFacade(this IProfileModifier modifier, ICommand<ModifierFacade> removeCommand)
        {
            return new ModifierFacade(modifier, removeCommand);
        }

        public static ModifierFacade ToFacade(this IRuleModifier modifier, ICommand<ModifierFacade> removeCommand)
        {
            return new ModifierFacade(modifier, removeCommand);
        }
    }
}
