// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using AppServices;
    using Bindables;
    using Model;
    using Models;

    public abstract class ModifierViewModelBase<TViewModel, TModifier, TValue, TAction, TField> :
        GenericViewModel<TViewModel, TModifier>, IConditionItemsListViewModel
        where TModifier : ICatalogueModifier<TValue, TAction, TField>
    {
        private bool _isFieldActive;
        private bool _isValueActive;

        protected ModifierViewModelBase(ICommandsAggregateService commands, TModifier model) : base(model)
        {
            if (commands == null)
                throw new ArgumentNullException(nameof(commands));
            Commands = commands;
            Repetition = new ModifierRepetitionViewModel(Modifier.Repetition);
            ConditionsMap = Modifier.Conditions.ToBindableMap("conditions",
                Commands.RemoveConditionItemCommand.For(() => ConditionsMap));
            GroupsMap = Modifier.ConditionGroups.ToBindableMap("condition groups",
                Commands.RemoveConditionItemCommand.For(() => GroupsMap));
        }

        public TAction Action
        {
            get { return Modifier.Action; }
            set
            {
                if (Set(() => EqualityComparer<TAction>.Default.Equals(Modifier.Action, value),
                    () => Modifier.Action = value))
                {
                    OnActionChanged();
                }
            }
        }

        public abstract IEnumerable<TAction> Actions { get; }

        public TField Field
        {
            get { return Modifier.Field; }
            set
            {
                if (Set(() => EqualityComparer<TField>.Default.Equals(Modifier.Field, value),
                    () => Modifier.Field = value))
                {
                    OnFieldChanged();
                }
            }
        }

        public abstract IEnumerable<TField> Fields { get; }

        public bool IsFieldActive
        {
            get { return _isFieldActive; }
            set { Set(ref _isFieldActive, value); }
        }

        public bool IsValueActive
        {
            get { return _isValueActive; }
            set { Set(ref _isValueActive, value); }
        }

        public ModifierRepetitionViewModel Repetition { get; }

        public TValue Value
        {
            get { return Modifier.Value; }
            set
            {
                if (Set(() => EqualityComparer<TValue>.Default.Equals(Modifier.Value, value),
                    () => Modifier.Value = value))
                {
                    OnValueChanged();
                }
            }
        }

        protected ICommandsAggregateService Commands { get; }

        protected TModifier Modifier => Model;

        private BindableMap<ConditionItemFacade, ICatalogueCondition> ConditionsMap { get; }

        private BindableMap<ConditionItemFacade, ICatalogueConditionGroup> GroupsMap { get; }

        public IEnumerable<IBindableGrouping<ConditionItemFacade>> ConditionItems
        {
            get
            {
                yield return ConditionsMap;
                yield return GroupsMap;
            }
        }

        public ICommand CreateConditionItemCommand => Commands.CreateConditionItemCommand.EnableFor(Modifier);

        public ICommand OpenConditionItemCommand => Commands.OpenConditionItemCommand;

        protected virtual void OnActionChanged()
        {
        }

        protected virtual void OnFieldChanged()
        {
        }

        protected virtual void OnValueChanged()
        {
        }
    }
}
