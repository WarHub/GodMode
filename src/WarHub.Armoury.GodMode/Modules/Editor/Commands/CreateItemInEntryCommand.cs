// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model;
    using Models;

    public delegate CreateItemInEntryCommand CreateItemInEntryCommandFactory(IEntry entry);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateItemInEntryCommand : CreateCatalogueItemCommandBase
    {
        public CreateItemInEntryCommand(IAppCommandDependencyAggregate dependencyAggregate,
            OpenCatalogueItemCommand openCatalogueItemCommand, IEntry entry)
            : base(dependencyAggregate, openCatalogueItemCommand)
        {
            Entry = entry;
        }

        protected override IEnumerable<ItemKind> ConfiguredKinds
        {
            get
            {
                yield return ItemKind.Entry;
                yield return ItemKind.Group;
                yield return ItemKind.Profile;
                yield return ItemKind.Rule;
                yield return ItemKind.EntryLink;
                yield return ItemKind.GroupLink;
                yield return ItemKind.ProfileLink;
                yield return ItemKind.RuleLink;
            }
        }

        private IEntry Entry { get; }

        protected override async Task<CatalogueItemFacade> CreateItemAsync(ItemKind itemKind)
        {
            var catalogue = Entry.Context.Catalogue;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (itemKind)
            {
                case ItemKind.Entry:
                    return Entry.Entries.AddNew().ToFacade(null);
                case ItemKind.Group:
                    return Entry.Groups.AddNew().ToFacade(null);
                case ItemKind.Profile:
                    return Entry.Profiles.AddNew().ToFacade(null);
                case ItemKind.Rule:
                    return Entry.Rules.AddNew().ToFacade(null);
                case ItemKind.EntryLink:
                {
                    var shared = catalogue.SharedEntries.FirstOrDefault();
                    if (shared == null)
                    {
                        await InformLinkTargetUnavailable("shared entry");
                        return null;
                    }
                    return Entry.EntryLinks.AddNew(shared).ToFacade(null);
                }
                case ItemKind.GroupLink:
                {
                    var shared = catalogue.SharedGroups.FirstOrDefault();
                    if (shared == null)
                    {
                        await InformLinkTargetUnavailable("shared group");
                        return null;
                    }
                    return Entry.GroupLinks.AddNew(shared).ToFacade(null);
                }
                case ItemKind.ProfileLink:
                {
                    var shared = catalogue.SharedProfiles.FirstOrDefault();
                    if (shared == null)
                    {
                        await InformLinkTargetUnavailable("shared profile");
                        return null;
                    }
                    return Entry.ProfileLinks.AddNew(shared).ToFacade(null);
                }
                case ItemKind.RuleLink:
                {
                    var shared = catalogue.SharedRules.FirstOrDefault();
                    if (shared == null)
                    {
                        await InformLinkTargetUnavailable("shared rule");
                        return null;
                    }
                    return Entry.RuleLinks.AddNew(shared).ToFacade(null);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }
    }
}
