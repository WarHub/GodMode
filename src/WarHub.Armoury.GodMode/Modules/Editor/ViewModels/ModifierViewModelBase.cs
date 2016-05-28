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

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class ModifierViewModelBase<TModifier, TValue, TAction, TField> :
        GenericViewModel<TModifier>, IConditionItemsListViewModel
        where TModifier : ICatalogueModifier<TValue, TAction, TField>
    {
        private bool _isFieldActive;
        private bool _isValueActive;

        protected ModifierViewModelBase(TModifier model,
            CreateConditionItemCommandFactory createConditionItemCommandFactory,
            OpenConditionItemCommand openConditionItemCommand,
            Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> removeCommandFactory)
            : base(model)
        {
            OpenConditionItemCommand = openConditionItemCommand;
            CreateConditionItemCommand = createConditionItemCommandFactory(Modifier);
            Repetition = new ModifierRepetitionViewModel(Modifier.Repetition);
            ConditionsMap = Modifier.Conditions.ToBindableMap("conditions", removeCommandFactory);
            GroupsMap = Modifier.ConditionGroups.ToBindableMap("condition groups", removeCommandFactory);
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

        public CreateConditionItemCommand CreateConditionItemCommand { get; }

        public OpenConditionItemCommand OpenConditionItemCommand { get; }

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
