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

    public delegate CreateItemInGroupCommand CreateItemInGroupCommandFactory(IGroup group);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateItemInGroupCommand : CreateCatalogueItemCommandBase
    {
        public CreateItemInGroupCommand(IAppCommandDependencyAggregate dependencyAggregate,
            OpenCatalogueItemCommand openCatalogueItemCommand, IGroup group)
            : base(dependencyAggregate, openCatalogueItemCommand)
        {
            Group = group;
        }

        protected override IEnumerable<ItemKind> ConfiguredKinds
        {
            get
            {
                yield return ItemKind.Entry;
                yield return ItemKind.Group;
                yield return ItemKind.EntryLink;
                yield return ItemKind.GroupLink;
            }
        }

        private IGroup Group { get; }

        protected override async Task<CatalogueItemFacade> CreateItemAsync(ItemKind itemKind)
        {
            var catalogue = Group.Context.Catalogue;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (itemKind)
            {
                case ItemKind.Entry:
                    return Group.Entries.AddNew().ToFacade(null);
                case ItemKind.Group:
                    return Group.Groups.AddNew().ToFacade(null);
                case ItemKind.EntryLink:
                {
                    var shared = catalogue.SharedEntries.FirstOrDefault();
                    if (shared == null)
                    {
                        await InformLinkTargetUnavailable("shared entry");
                        return null;
                    }
                    return Group.EntryLinks.AddNew(shared).ToFacade(null);
                }
                case ItemKind.GroupLink:
                {
                    var shared = catalogue.SharedGroups.FirstOrDefault();
                    if (shared == null)
                    {
                        await InformLinkTargetUnavailable("shared group");
                        return null;
                    }
                    return Group.GroupLinks.AddNew(shared).ToFacade(null);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }
    }
}
