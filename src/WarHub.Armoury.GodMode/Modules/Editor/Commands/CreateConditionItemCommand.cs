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

    public delegate CreateConditionItemCommand CreateConditionItemCommandFactory(
        ICatalogueConditionNodeContainer nodeContainer);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateConditionItemCommand : AppAsyncCommandBase
    {
        public CreateConditionItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            ICatalogueConditionNodeContainer nodeContainer, OpenConditionItemCommand openConditionItemCommand)
            : base(dependencyAggregate)
        {
            NodeContainer = nodeContainer;
            OpenConditionItemCommand = openConditionItemCommand;
        }

        private static Dictionary<ItemKind, string> ItemKindNames { get; } = new Dictionary<ItemKind, string>
        {
            [ItemKind.Condition] = "Condition",
            [ItemKind.ConditionGroup] = "Condition group"
        };

        private static Dictionary<string, ItemKind> ItemKinds { get; } =
            ItemKindNames.Keys.ToDictionary(kind => ItemKindNames[kind]);

        private ICatalogueConditionNodeContainer NodeContainer { get; }

        private OpenConditionItemCommand OpenConditionItemCommand { get; }

        protected override async Task ExecuteCoreAsync()
        {
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
                    return NodeContainer.Conditions.AddNew().ToFacade(null);
                case ItemKind.ConditionGroup:
                    return NodeContainer.ConditionGroups.AddNew().ToFacade(null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static IEnumerable<ItemKind> GetKinds()
        {
            yield return ItemKind.Condition;
            yield return ItemKind.ConditionGroup;
        }

        protected override bool CanExecuteCore() => NodeContainer != null;

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
