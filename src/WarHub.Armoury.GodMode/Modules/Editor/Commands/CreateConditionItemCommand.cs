// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AppServices;
    using Model;
    using Models;
    using Mvvm.Commands;

    public class CreateConditionItemCommand : ProgressingAsyncCommandBase
    {
        public CreateConditionItemCommand(IDialogService dialogService,
            OpenConditionItemCommand openConditionItemCommand, ICatalogueConditionNodeContainer nodeContainer = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            if (openConditionItemCommand == null)
                throw new ArgumentNullException(nameof(openConditionItemCommand));
            DialogService = dialogService;
            OpenConditionItemCommand = openConditionItemCommand;
            NodeContainer = nodeContainer;
        }

        private IDialogService DialogService { get; }

        private static Dictionary<ItemKind, string> ItemKindNames { get; } = new Dictionary<ItemKind, string>
        {
            [ItemKind.Condition] = "Condition",
            [ItemKind.ConditionGroup] = "Condition group"
        };

        private static Dictionary<string, ItemKind> ItemKinds { get; } =
            ItemKindNames.Keys.ToDictionary(kind => ItemKindNames[kind]);

        private ICatalogueConditionNodeContainer NodeContainer { get; }

        private OpenConditionItemCommand OpenConditionItemCommand { get; }

        public CreateConditionItemCommand EnableFor(ICatalogueConditionNodeContainer nodeContainer)
        {
            return new CreateConditionItemCommand(DialogService, OpenConditionItemCommand, nodeContainer);
        }

        protected override async Task ExecuteCoreAsync()
        {
            if (NodeContainer == null)
            {
                await InformRequestCannotBeProcessedAsync();
                return;
            }
            var kindNames = GetKinds().Select(kind => ItemKindNames[kind]).ToArray();
            var chosenName = await QueryUserForItemKind(kindNames);
            if (!ItemKinds.ContainsKey(chosenName))
            {
                return;
            }
            var chosenKind = ItemKinds[chosenName];
            var item = CreateItem(chosenKind);
            if (item == null)
            {
                return;
            }
            if (OpenConditionItemCommand.CanExecute(item))
            {
                OpenConditionItemCommand.Execute(item);
                return;
            }
            await
                DialogService.ShowDialogAsync("Cannot navigate", "There was an error and the item could not be opened.",
                    "cancel");
        }

        private ConditionItemFacade CreateItem(ItemKind kind)
        {
            switch (kind)
            {
                case ItemKind.Condition:
                    return NodeContainer.Conditions.AddNew().ToFacade();
                case ItemKind.ConditionGroup:
                    return NodeContainer.ConditionGroups.AddNew().ToFacade();
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private IEnumerable<ItemKind> GetKinds()
        {
            yield return ItemKind.Condition;
            yield return ItemKind.ConditionGroup;
        }

        protected override bool CanExecuteCore() => NodeContainer != null;

        private async Task InformRequestCannotBeProcessedAsync()
        {
            await
                DialogService.ShowDialogAsync("Ooops",
                    "Something's wrong: the request cannot be processed." +
                    " Additional info: the command was not set up properly.", "ok");
        }

        private async Task<string> QueryUserForItemKind(string[] configuredKindNames)
        {
            return
                await DialogService.ShowOptionsAsync("Choose item type to create", "cancel", null, configuredKindNames);
        }

        private enum ItemKind
        {
            Condition,
            ConditionGroup
        }
    }
}
