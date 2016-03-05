// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System.Collections.Generic;
    using Demo;
    using Model;
    using Services;

    public class EntryModifierViewModel :
        ModifierViewModelBase<EntryModifierViewModel, IEntryModifier, decimal, EntryBaseModifierAction, EntryField>
    {
        public EntryModifierViewModel(ICommandsAggregateService commands, IEntryModifier model = null)
            : base(commands, model ?? ModelLocator.EntryModifier)
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

        protected override EntryModifierViewModel WithModelCore(IEntryModifier model)
        {
            return new EntryModifierViewModel(Commands, model);
        }
    }
}
