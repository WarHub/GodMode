// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using AppServices;
    using Model;

    public class EntryModifierViewModel :
        ModifierViewModelBase<IEntryModifier, decimal, EntryBaseModifierAction, EntryField>
    {
        public EntryModifierViewModel(IEntryModifier model, ICommandsAggregateService commands)
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

        public override IEnumerable<EntryField> Fields
        {
            get
            {
                yield return EntryField.MinSelections;
                yield return EntryField.MaxSelections;
                yield return EntryField.MinInForce;
                yield return EntryField.MaxInForce;
                yield return EntryField.MinInRoster;
                yield return EntryField.MaxInRoster;
                yield return EntryField.MinPoints;
                yield return EntryField.MaxPoints;
                yield return EntryField.PointCost;
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
