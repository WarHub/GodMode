// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using Model;
    using Mvvm.Commands;

    public static class CatalogueItemFacadeExtensions
    {
        #region Shared

        public static CatalogueItemFacade ToFacadeShared(this IEntry entry, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(entry, CatalogueItemKind.Entry, removeCommand, () => entry.Name,
                isShared: true);
        }

        public static CatalogueItemFacade ToFacadeShared(this IGroup group, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(group, CatalogueItemKind.Group, removeCommand, () => group.Name,
                isShared: true);
        }

        public static CatalogueItemFacade ToFacadeShared(this IProfile profile, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(profile, CatalogueItemKind.Profile, removeCommand, () => profile.Name,
                isShared: true);
        }

        public static CatalogueItemFacade ToFacadeShared(this IRule rule, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(rule, CatalogueItemKind.Rule, removeCommand, () => rule.Name, 
                isShared: true);
        }

        #endregion

        #region Not Shared

        public static CatalogueItemFacade ToFacade(this IEntry entry, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(entry, CatalogueItemKind.Entry, removeCommand, () => entry.Name);
        }

        public static CatalogueItemFacade ToFacade(this IGroup group, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(group, CatalogueItemKind.Group, removeCommand, () => group.Name);
        }

        public static CatalogueItemFacade ToFacade(this IProfile profile, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(profile, CatalogueItemKind.Profile, removeCommand, () => profile.Name);
        }

        public static CatalogueItemFacade ToFacade(this IRule rule, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(rule, CatalogueItemKind.Rule, removeCommand, () => rule.Name);
        }

        #endregion

        #region Links

        public static CatalogueItemFacade ToFacade(this IEntryLink entryLink,
            ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(entryLink, CatalogueItemKind.Entry, removeCommand,
                () => entryLink.Target.Name, isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IGroupLink groupLink,
            ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(groupLink, CatalogueItemKind.Group, removeCommand,
                () => groupLink.Target.Name, isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IProfileLink profileLink,
            ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(profileLink, CatalogueItemKind.Profile, removeCommand,
                () => profileLink.Target.Name,
                isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IRuleLink ruleLink, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(ruleLink, CatalogueItemKind.Rule, removeCommand, () => ruleLink.Target.Name,
                isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IRootEntry rootEntry,
            ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(rootEntry, CatalogueItemKind.Entry, removeCommand, () => rootEntry.Name);
        }

        public static CatalogueItemFacade ToFacade(this IRootLink rootLink, ICommand<CatalogueItemFacade> removeCommand)
        {
            return new CatalogueItemFacade(rootLink, CatalogueItemKind.Entry, removeCommand, () => rootLink.Target.Name,
                isLink: true);
        }

        #endregion

    }
}
