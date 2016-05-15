// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using AppServices;
    using Model;

    public class GroupModifierViewModel :
        ModifierViewModelBase<IGroupModifier, decimal, EntryBaseModifierAction, GroupField>
    {
        public GroupModifierViewModel(IGroupModifier model, ICommandsAggregateService commands)
            : base(model, commands)
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
