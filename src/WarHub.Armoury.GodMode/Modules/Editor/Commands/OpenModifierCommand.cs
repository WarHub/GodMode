// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using GodMode.Commands;
    using Model;
    using Models;
    using Services;
    using ViewModels;
    using Views;

    public class OpenModifierCommand : NavigateCommandBase<ModifierFacade>
    {
        public OpenModifierCommand(IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
        }

        protected override bool CanExecuteCore(ModifierFacade parameter) => parameter != null;

        protected override NavTuple GetNavTuple(ModifierFacade facade)
        {
            switch (facade.Kind)
            {
                case ModifierKind.Entry:
                    return new NavTuple(new EntryModifierPage(),
                        ViewModelLocator.EntryModifierViewModel.WithModel((IEntryModifier) facade.Model));
                case ModifierKind.Group:
                    return new NavTuple(new GroupModifierPage(),
                        ViewModelLocator.GroupModifierViewModel.WithModel((IGroupModifier) facade.Model));
                case ModifierKind.Profile:
                    return new NavTuple(new ProfileModifierPage(),
                        ViewModelLocator.ProfileModifierViewModel.WithModel((IProfileModifier) facade.Model));
                case ModifierKind.Rule:
                    return new NavTuple(new RuleModifierPage(),
                        ViewModelLocator.RuleModifierViewModel.WithModel((IRuleModifier) facade.Model));
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
