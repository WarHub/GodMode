// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model;
    using Models;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class CreateCatalogueItemCommandBase : AppAsyncCommandBase
    {
        protected CreateCatalogueItemCommandBase(IAppCommandDependencyAggregate dependencyAggregate,
            OpenCatalogueItemCommand openCatalogueItemCommand)
            : base(dependencyAggregate)
        {
            OpenCatalogueItemCommand = openCatalogueItemCommand;
        }

        protected abstract IEnumerable<ItemKind> ConfiguredKinds { get; }

        private static IReadOnlyDictionary<ItemKind, string> ItemKindNames { get; } = new Dictionary<ItemKind, string>
        {
            [ItemKind.Entry] = "Entry",
            [ItemKind.Group] = "Group",
            [ItemKind.Profile] = "Profile",
            [ItemKind.Rule] = "Rule",
            [ItemKind.EntryLink] = "Entry link",
            [ItemKind.GroupLink] = "Group link",
            [ItemKind.ProfileLink] = "Profile link",
            [ItemKind.RuleLink] = "Rule link",
            [ItemKind.SharedEntry] = "Shared Entry",
            [ItemKind.SharedGroup] = "Shared Group",
            [ItemKind.SharedProfile] = "Shared Profile",
            [ItemKind.SharedRule] = "Shared Rule"
        };

        private static IReadOnlyDictionary<string, ItemKind> ItemKinds { get; } =
            ItemKindNames.Keys.ToDictionary(kind => ItemKindNames[kind]);

        private OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }

        protected abstract Task<CatalogueItemFacade> CreateItemAsync(ItemKind itemKind);

        protected override async Task ExecuteCoreAsync()
        {
            var configuredKindNames = ConfiguredKinds.Select(kind => ItemKindNames[kind]).ToArray();
            var chosenKindName = await QueryUserForItemKind(configuredKindNames);
            if (!ItemKinds.ContainsKey(chosenKindName))
            {
                return;
            }
            var chosenKind = ItemKinds[chosenKindName];
            var item = await CreateItemAsync(chosenKind);
            if (item == null)
            {
                return;
            }
            ConfigureItemName(item, chosenKindName);
            if (OpenCatalogueItemCommand.CanExecute(item))
            {
                OpenCatalogueItemCommand.Execute(item);
                return;
            }
            await
                DialogService.ShowDialogAsync("Cannot navigate", "There was an error and the item could not be opened.",
                    "cancel");
        }

        private static void ConfigureItemName(CatalogueItemFacade facade, string chosenKindName)
        {
            var nameable = facade.Item as INameable;
            if (nameable != null && !facade.IsLink)
            {
                nameable.Name = $"New {chosenKindName}";
            }
        }

        protected async Task InformLinkTargetUnavailable(string prerequisite)
        {
            await
                DialogService.ShowDialogAsync("No target",
                    $"Link cannot be created. Please create {prerequisite} first.", "cancel");
        }

        private async Task<string> QueryUserForItemKind(string[] configuredKindNames)
        {
            return
                await DialogService.ShowOptionsAsync("Choose item type to create", "cancel", null, configuredKindNames);
        }

        protected enum ItemKind

        {
            Entry,
            Group,
            Profile,
            Rule,
            EntryLink,
            GroupLink,
            ProfileLink,
            RuleLink,
            SharedEntry,
            SharedGroup,
            SharedProfile,
            SharedRule
        }
    }
}
