// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using AppServices;
    using GodMode.Commands;
    using Model;
    using Models;
    using ViewModels;
    using Views;

    public class OpenModifierCommand : NavigateCommandBase<ModifierFacade>
    {
        public OpenModifierCommand(IDialogService dialogService, INavigationService navigationService,
            Func<IEntryModifier, EntryModifierViewModel> entryModifierVmFactory,
            Func<IGroupModifier, GroupModifierViewModel> groupModifierVmFactory,
            Func<IProfileModifier, ProfileModifierViewModel> profileModifierVmFactory,
            Func<IRuleModifier, RuleModifierViewModel> ruleModifierVmFactory)
            : base(dialogService, navigationService)
        {
            EntryModifierVmFactory = entryModifierVmFactory;
            GroupModifierVmFactory = groupModifierVmFactory;
            ProfileModifierVmFactory = profileModifierVmFactory;
            RuleModifierVmFactory = ruleModifierVmFactory;
        }

        private Func<IEntryModifier, EntryModifierViewModel> EntryModifierVmFactory { get; }

        private Func<IGroupModifier, GroupModifierViewModel> GroupModifierVmFactory { get; }

        private Func<IProfileModifier, ProfileModifierViewModel> ProfileModifierVmFactory { get; }

        private Func<IRuleModifier, RuleModifierViewModel> RuleModifierVmFactory { get; }

        protected override bool CanExecuteCore(ModifierFacade parameter) => parameter != null;

        protected override NavTuple GetNavTuple(ModifierFacade facade)
        {
            switch (facade.Kind)
            {
                case ModifierKind.Entry:
                    return new NavTuple(new EntryModifierPage(), EntryModifierVmFactory((IEntryModifier) facade.Model));
                case ModifierKind.Group:
                    return new NavTuple(new GroupModifierPage(), GroupModifierVmFactory((IGroupModifier) facade.Model));
                case ModifierKind.Profile:
                    return new NavTuple(new ProfileModifierPage(),
                        ProfileModifierVmFactory((IProfileModifier) facade.Model));
                case ModifierKind.Rule:
                    return new NavTuple(new RuleModifierPage(), RuleModifierVmFactory((IRuleModifier) facade.Model));
                default:
                    return null;
            }
        }

        protected override string GetErrorString(ModifierFacade parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }
    }
}
