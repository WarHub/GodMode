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
    public class GroupModifierViewModel :
        ModifierViewModelBase<IGroupModifier, decimal, EntryBaseModifierAction, GroupField>
    {
        public GroupModifierViewModel(IGroupModifier model,
            CreateConditionItemCommandFactory createConditionItemCommandFactory,
            OpenConditionItemCommand openConditionItemCommand,
            Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> removeCommandFactory)
            : base(model, createConditionItemCommandFactory, openConditionItemCommand, removeCommandFactory)
        {
            IsFieldActive = CanFieldBeActive();
            IsValueActive = CanValueBeActive();
        }

        public override IEnumerable<EntryBaseModifierAction> Actions
        {
            get
            {
                yield return EntryBaseModifierAction.Increment;
                yield return EntryBaseModifierAction.Decrement;
                yield return EntryBaseModifierAction.Set;
                yield return EntryBaseModifierAction.Hide;
                yield return EntryBaseModifierAction.Show;
            }
        }

        public override IEnumerable<GroupField> Fields
        {
            get
            {
                yield return GroupField.MinSelections;
                yield return GroupField.MaxSelections;
                yield return GroupField.MinInForce;
                yield return GroupField.MaxInForce;
                yield return GroupField.MinInRoster;
                yield return GroupField.MaxInRoster;
                yield return GroupField.MinPoints;
                yield return GroupField.MaxPoints;
            }
        }

        protected override void OnActionChanged()
        {
            base.OnActionChanged();
            IsFieldActive = CanFieldBeActive();
            IsValueActive = CanValueBeActive();
        }

        private bool CanFieldBeActive()
            => Action != EntryBaseModifierAction.Hide && Action != EntryBaseModifierAction.Show;

        private bool CanValueBeActive() => CanFieldBeActive();
    }
}
