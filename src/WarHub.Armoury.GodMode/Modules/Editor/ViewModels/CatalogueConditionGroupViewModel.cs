// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Bindables;
    using Commands;
    using Model;
    using Models;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CatalogueConditionGroupViewModel : GenericViewModel<ICatalogueConditionGroup>,
        IConditionItemsListViewModel
    {
        public CatalogueConditionGroupViewModel(ICatalogueConditionGroup model,
            Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> removeCommandFactory,
            CreateConditionItemCommandFactory createConditionItemCommandFactory,
            OpenConditionItemCommand openConditionItemCommand)
            : base(model)
        {
            OpenConditionItemCommand = openConditionItemCommand;
            CreateConditionItemCommand = createConditionItemCommandFactory(Group);
            ConditionsMap = Group.Conditions.ToBindableMap("conditions", removeCommandFactory);
            GroupsMap = Group.ConditionGroups.ToBindableMap("condition groups", removeCommandFactory);
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

        public CreateConditionItemCommand CreateConditionItemCommand { get; }

        public OpenConditionItemCommand OpenConditionItemCommand { get; }
    }
}
