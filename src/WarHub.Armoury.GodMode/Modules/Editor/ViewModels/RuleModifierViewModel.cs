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
    public class RuleModifierViewModel :
        ModifierViewModelBase<IRuleModifier, string, RuleModifierAction, RuleField>
    {
        public RuleModifierViewModel(IRuleModifier model,
            CreateConditionItemCommandFactory createConditionItemCommandFactory,
            OpenConditionItemCommand openConditionItemCommand,
            Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> removeCommandFactory)
            : base(model, createConditionItemCommandFactory, openConditionItemCommand, removeCommandFactory)
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
