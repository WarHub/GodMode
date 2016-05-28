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

    public delegate CreateItemInCatalogueCommand CreateItemInCatalogueCommandFactory(ICatalogue catalogue);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateItemInCatalogueCommand : CreateCatalogueItemCommandBase
    {
        public CreateItemInCatalogueCommand(IAppCommandDependencyAggregate dependencyAggregate,
            OpenCatalogueItemCommand openCatalogueItemCommand, ICatalogue catalogue)
            : base(dependencyAggregate, openCatalogueItemCommand)
        {
            Catalogue = catalogue;
        }

        protected override IEnumerable<ItemKind> ConfiguredKinds
        {
            get
            {
                yield return ItemKind.Entry;
                yield return ItemKind.EntryLink;
                yield return ItemKind.SharedEntry;
                yield return ItemKind.SharedGroup;
                yield return ItemKind.SharedProfile;
                yield return ItemKind.SharedRule;
            }
        }

        private ICatalogue Catalogue { get; }

        protected override async Task<CatalogueItemFacade> CreateItemAsync(ItemKind itemKind)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (itemKind)
            {
                case ItemKind.Rule:
                    return Catalogue.Rules.AddNew().ToFacade(null);
                case ItemKind.RuleLink:
                    if (Catalogue.SharedRules.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared rule");
                        return null;
                    }
                    return Catalogue.RuleLinks.AddNew(Catalogue.SharedRules.First()).ToFacade(null);
                case ItemKind.Entry:
                    return Catalogue.Entries.AddNew().ToFacade(null);
                case ItemKind.EntryLink:
                    if (Catalogue.SharedEntries.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared entry");
                        return null;
                    }
                    return Catalogue.EntryLinks.AddNew(Catalogue.SharedEntries.First()).ToFacade(null);
                case ItemKind.SharedEntry:
                    return Catalogue.SharedEntries.AddNew().ToFacadeShared(null);
                case ItemKind.SharedGroup:
                    return Catalogue.SharedGroups.AddNew().ToFacadeShared(null);
                case ItemKind.SharedProfile:
                    return Catalogue.SharedProfiles.AddNew().ToFacadeShared(null);
                case ItemKind.SharedRule:
                    return Catalogue.SharedRules.AddNew().ToFacadeShared(null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }
    }
}
