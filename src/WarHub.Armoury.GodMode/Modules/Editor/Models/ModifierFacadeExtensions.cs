// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using Model;

    public static class ModifierFacadeExtensions
    {
        public static ModifierFacade ToFacade(this IEntryModifier modifier)
        {
            return new ModifierFacade(modifier);
        }

        public static ModifierFacade ToFacade(this IGroupModifier modifier)
        {
            return new ModifierFacade(modifier);
        }

        public static ModifierFacade ToFacade(this IProfileModifier modifier)
        {
            return new ModifierFacade(modifier);
        }

        public static ModifierFacade ToFacade(this IRuleModifier modifier)
        {
            return new ModifierFacade(modifier);
        }
    }
}
