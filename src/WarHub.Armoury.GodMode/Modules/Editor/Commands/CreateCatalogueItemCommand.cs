// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model;
    using Models;
    using Mvvm.Commands;
    using Services;

    public class CreateCatalogueItemCommand : ProgressingAsyncCommandBase
    {
        public CreateCatalogueItemCommand(IDialogService dialogService,
            OpenCatalogueItemCommand openCatalogueItemCommand) : this(dialogService, openCatalogueItemCommand, null)
        {
        }

        private CreateCatalogueItemCommand(IDialogService dialogService,
            OpenCatalogueItemCommand openCatalogueItemCommand,
            Func<ItemKind, Task<CatalogueItemFacade>> createItemAsync = null,
            Func<ItemKind[]> getConfiguredKindsFunc = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            if (openCatalogueItemCommand == null)
                throw new ArgumentNullException(nameof(openCatalogueItemCommand));
            DialogService = dialogService;
            OpenCatalogueItemCommand = openCatalogueItemCommand;
            CreateItemAsync = createItemAsync;
            GetConfiguredKindsFunc = getConfiguredKindsFunc;
        }

        private Func<ItemKind, Task<CatalogueItemFacade>> CreateItemAsync { get; }

        private IDialogService DialogService { get; }

        private Func<ItemKind[]> GetConfiguredKindsFunc { get; }

        private static Dictionary<ItemKind, string> ItemKindNames { get; } = new Dictionary<ItemKind, string>
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

        private static Dictionary<string, ItemKind> ItemKinds { get; } =
            ItemKindNames.Keys.ToDictionary(kind => ItemKindNames[kind]);

        private OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }

        public CreateCatalogueItemCommand EnableFor(ICatalogue catalogue)
        {
            return new CreateCatalogueItemCommand(DialogService, OpenCatalogueItemCommand,
                kind => CreateItem(kind, catalogue), GetCatalogueKinds);
        }

        public CreateCatalogueItemCommand EnableFor(IEntry entry)
        {
            return new CreateCatalogueItemCommand(DialogService, OpenCatalogueItemCommand,
                kind => CreateItem(kind, entry), GetEntryKinds);
        }

        public CreateCatalogueItemCommand EnableFor(IGroup group)
        {
            return new CreateCatalogueItemCommand(DialogService, OpenCatalogueItemCommand,
                kind => CreateItem(kind, group), GetGroupKinds);
        }

        protected override async Task ExecuteCoreAsync()
        {
            if (CreateItemAsync == null || GetConfiguredKindsFunc == null)
            {
                await InformRequestCannotBeProcessedAsync();
                return;
            }
            var configuredKinds = GetConfiguredKindsFunc();
            var configuredKindNames = configuredKinds.Select(kind => ItemKindNames[kind]).ToArray();
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
            ConfiureItem(item, chosenKindName);
            if (OpenCatalogueItemCommand.CanExecute(item))
            {
                OpenCatalogueItemCommand.Execute(item);
                return;
            }
            await
                DialogService.ShowDialogAsync("Cannot navigate", "There was an error and the item could not be opened.",
                    "cancel");
        }

        private static void ConfiureItem(CatalogueItemFacade facade, string chosenKindName)
        {
            var nameable = facade.Item as INameable;
            if (nameable != null && !facade.IsLink)
            {
                nameable.Name = $"New {chosenKindName}";
            }
        }

        private async Task InformRequestCannotBeProcessedAsync()
        {
            await
                DialogService.ShowDialogAsync("Ooops",
                    "Something's wrong: the request cannot be processed." +
                    " Additional info: the command was not set up properly.",
                    "ok");
        }

        private async Task<CatalogueItemFacade> CreateItem(ItemKind itemKind, ICatalogue catalogue)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (itemKind)
            {
                case ItemKind.Rule:
                    return catalogue.Rules.AddNew().ToFacade();
                case ItemKind.RuleLink:
                    if (catalogue.SharedRules.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared rule");
                        return null;
                    }
                    return catalogue.RuleLinks.AddNew(catalogue.SharedRules.First()).ToFacade();
                case ItemKind.Entry:
                    return catalogue.Entries.AddNew().ToFacade();
                case ItemKind.EntryLink:
                    if (catalogue.SharedEntries.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared entry");
                        return null;
                    }
                    return catalogue.EntryLinks.AddNew(catalogue.SharedEntries.First()).ToFacade();
                case ItemKind.SharedEntry:
                    return catalogue.SharedEntries.AddNew().ToFacade(true);
                case ItemKind.SharedGroup:
                    return catalogue.SharedGroups.AddNew().ToFacade(true);
                case ItemKind.SharedProfile:
                    return catalogue.SharedProfiles.AddNew().ToFacade(true);
                case ItemKind.SharedRule:
                    return catalogue.SharedRules.AddNew().ToFacade(true);
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }

        private async Task<CatalogueItemFacade> CreateItem(ItemKind itemKind, IEntry entry)
        {
            var catalogue = entry.Context.Catalogue;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (itemKind)
            {
                case ItemKind.Entry:
                    return entry.Entries.AddNew().ToFacade();
                case ItemKind.Group:
                    return entry.Groups.AddNew().ToFacade();
                case ItemKind.Profile:
                    return entry.Profiles.AddNew().ToFacade();
                case ItemKind.Rule:
                    return entry.Rules.AddNew().ToFacade();
                case ItemKind.EntryLink:
                    if (catalogue.SharedEntries.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared entry");
                        return null;
                    }
                    return entry.EntryLinks.AddNew(catalogue.SharedEntries.First()).ToFacade();
                case ItemKind.GroupLink:
                    if (catalogue.SharedGroups.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared group");
                        return null;
                    }
                    return entry.GroupLinks.AddNew(catalogue.SharedGroups.First()).ToFacade();
                case ItemKind.ProfileLink:
                    if (catalogue.SharedProfiles.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared profile");
                        return null;
                    }
                    return entry.ProfileLinks.AddNew(catalogue.SharedProfiles.First()).ToFacade();
                case ItemKind.RuleLink:
                    if (catalogue.SharedRules.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared rule");
                        return null;
                    }
                    return entry.RuleLinks.AddNew(catalogue.SharedRules.First()).ToFacade();
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }

        private async Task<CatalogueItemFacade> CreateItem(ItemKind itemKind, IGroup entry)
        {
            var catalogue = entry.Context.Catalogue;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (itemKind)
            {
                case ItemKind.Entry:
                    return entry.Entries.AddNew().ToFacade();
                case ItemKind.Group:
                    return entry.Groups.AddNew().ToFacade();
                case ItemKind.EntryLink:
                    if (catalogue.SharedEntries.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared entry");
                        return null;
                    }
                    return entry.EntryLinks.AddNew(catalogue.SharedEntries.First()).ToFacade();
                case ItemKind.GroupLink:
                    if (catalogue.SharedGroups.Count < 1)
                    {
                        await InformLinkTargetUnavailable("shared group");
                        return null;
                    }
                    return entry.GroupLinks.AddNew(catalogue.SharedGroups.First()).ToFacade();
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }

        private async Task InformLinkTargetUnavailable(string prerequisite)
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

        private static ItemKind[] GetCatalogueKinds()
        {
            return new[]
            {
                ItemKind.Entry, ItemKind.EntryLink, ItemKind.SharedEntry, ItemKind.SharedGroup,
                ItemKind.SharedProfile, ItemKind.SharedRule
            };
        }

        private static ItemKind[] GetEntryKinds()
        {
            return new[]
            {
                ItemKind.Entry, ItemKind.Group, ItemKind.Profile, ItemKind.Rule, ItemKind.EntryLink, ItemKind.GroupLink,
                ItemKind.ProfileLink, ItemKind.RuleLink
            };
        }

        private static ItemKind[] GetGroupKinds()
        {
            return new[]
            {
                ItemKind.Entry, ItemKind.Group, ItemKind.EntryLink, ItemKind.GroupLink
            };
        }

        private enum ItemKind

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
