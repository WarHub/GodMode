// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ModelFacades
{
    using Model;

    public static class CatalogueItemFacadeExtensions
    {
        public static CatalogueItemFacade ToFacade(this IEntry entry, bool isShared = false)
        {
            return new CatalogueItemFacade(entry, CatalogueItemKind.Entry, () => entry.Name, isShared: isShared);
        }

        public static CatalogueItemFacade ToFacade(this IGroup group, bool isShared = false)
        {
            return new CatalogueItemFacade(group, CatalogueItemKind.Group, () => group.Name, isShared: isShared);
        }

        public static CatalogueItemFacade ToFacade(this IProfile profile, bool isShared = false)
        {
            return new CatalogueItemFacade(profile, CatalogueItemKind.Profile, () => profile.Name, isShared: isShared);
        }

        public static CatalogueItemFacade ToFacade(this IRule rule, bool isShared = false)
        {
            return new CatalogueItemFacade(rule, CatalogueItemKind.Rule, () => rule.Name, isShared: isShared);
        }

        public static CatalogueItemFacade ToFacade(this IEntryLink entryLink)
        {
            return new CatalogueItemFacade(entryLink, CatalogueItemKind.Entry, () => entryLink.Target.Name, isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IGroupLink groupLink)
        {
            return new CatalogueItemFacade(groupLink, CatalogueItemKind.Group, () => groupLink.Target.Name, isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IProfileLink profileLink)
        {
            return new CatalogueItemFacade(profileLink, CatalogueItemKind.Profile, () => profileLink.Target.Name,
                isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IRuleLink ruleLink)
        {
            return new CatalogueItemFacade(ruleLink, CatalogueItemKind.Rule, () => ruleLink.Target.Name, isLink: true);
        }

        public static CatalogueItemFacade ToFacade(this IRootEntry rootEntry)
        {
            return new CatalogueItemFacade(rootEntry, CatalogueItemKind.Entry, () => rootEntry.Name);
        }

        public static CatalogueItemFacade ToFacade(this IRootLink rootLink)
        {
            return new CatalogueItemFacade(rootLink, CatalogueItemKind.Entry, () => rootLink.Target.Name, isLink: true);
        }
    }
}
