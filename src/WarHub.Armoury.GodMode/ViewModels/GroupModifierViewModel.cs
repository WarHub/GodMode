// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System.Collections.Generic;
    using Demo;
    using Model;
    using Services;

    public class GroupModifierViewModel :
        ModifierViewModelBase<GroupModifierViewModel, IGroupModifier, decimal, EntryBaseModifierAction, GroupField>
    {
        public GroupModifierViewModel(ICommandsAggregateService commands, IGroupModifier model = null)
            : base(commands, model ?? ModelLocator.GroupModifier)
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

        protected override GroupModifierViewModel WithModelCore(IGroupModifier model)
        {
            return new GroupModifierViewModel(Commands, model);
        }
    }
}
