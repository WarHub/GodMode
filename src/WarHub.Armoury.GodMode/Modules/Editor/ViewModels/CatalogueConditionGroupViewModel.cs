// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Bindables;
    using Demo;
    using Model;
    using Models;
    using Services;

    public class CatalogueConditionGroupViewModel :
        GenericViewModel<CatalogueConditionGroupViewModel, ICatalogueConditionGroup>,
        IConditionItemsListViewModel
    {
        public CatalogueConditionGroupViewModel(ICommandsAggregateService commands,
            ICatalogueConditionGroup model = null)
            : base(model ?? ModelLocator.CatalogueConditionGroup)
        {
            Commands = commands;
            ConditionsMap = Group.Conditions.ToBindableMap("conditions",
                Commands.RemoveConditionItemCommand.For(() => ConditionsMap));
            GroupsMap = Group.ConditionGroups.ToBindableMap("condition groups",
                Commands.RemoveConditionItemCommand.For(() => GroupsMap));
        }

        public ConditionGroupType Kind
        {
            get { return Group.Type; }
            set { Set(() => Group.Type == value, () => Group.Type = value); }
        }

        public IEnumerable<ConditionGroupType> Kinds
        {
            get
            {
                yield return ConditionGroupType.And;
                yield return ConditionGroupType.Or;
            }
        }

        private ICommandsAggregateService Commands { get; }

        private BindableMap<ConditionItemFacade, ICatalogueCondition> ConditionsMap { get; }

        private ICatalogueConditionGroup Group => Model;

        private BindableMap<ConditionItemFacade, ICatalogueConditionGroup> GroupsMap { get; }

        public IEnumerable<IBindableGrouping<ConditionItemFacade>> ConditionItems
        {
            get
            {
                yield return ConditionsMap;
                yield return GroupsMap;
            }
        }

        public ICommand CreateConditionItemCommand => Commands.CreateConditionItemCommand.EnableFor(Group);

        public ICommand OpenConditionItemCommand => Commands.OpenConditionItemCommand;

        protected override CatalogueConditionGroupViewModel WithModelCore(ICatalogueConditionGroup model)
        {
            return new CatalogueConditionGroupViewModel(Commands, model);
        }
    }
}
