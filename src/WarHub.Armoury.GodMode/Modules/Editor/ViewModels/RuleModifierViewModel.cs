// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using AppServices;
    using Model;

    public class RuleModifierViewModel :
        ModifierViewModelBase<IRuleModifier, string, RuleModifierAction, RuleField>
    {
        public RuleModifierViewModel(IRuleModifier model, ICommandsAggregateService commands)
            : base(model, commands)
        {
            IsFieldActive = CanFieldBeActive();
            IsValueActive = CanValueBeActive();
        }

        public override IEnumerable<RuleModifierAction> Actions
        {
            get
            {
                yield return RuleModifierAction.Set;
                yield return RuleModifierAction.Append;
                yield return RuleModifierAction.Hide;
                yield return RuleModifierAction.Show;
            }
        }

        public override IEnumerable<RuleField> Fields
        {
            get
            {
                yield return RuleField.Name;
                yield return RuleField.Description;
            }
        }

        protected override void OnActionChanged()
        {
            base.OnActionChanged();
            IsFieldActive = CanFieldBeActive();
            IsValueActive = CanValueBeActive();
        }

        private bool CanFieldBeActive() => Action != RuleModifierAction.Hide && Action != RuleModifierAction.Show;

        private bool CanValueBeActive() => CanFieldBeActive();
    }
}
