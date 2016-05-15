// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using GodMode.Commands;
    using Model;
    using Modules.Editor.Models;
    using Modules.Editor.ViewModels;
    using Modules.Editor.Views;
    using Services;

    public class OpenLinkTargetAsChildCommand : NavigateCommandBase<CatalogueItemFacade>
    {
        public OpenLinkTargetAsChildCommand(IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
        }

        protected override NavTuple GetNavTuple(CatalogueItemFacade facade)
        {
            switch (facade.ItemKind)
            {
                case CatalogueItemKind.Entry:
                    return new NavTuple(new EntryPage(),
                        ViewModelLocator.EntryViewModel.WithModel(((IEntryLink) facade.Item).Target));
                case CatalogueItemKind.Group:
                    return new NavTuple(new GroupPage(),
                        ViewModelLocator.GroupViewModel.WithModel(((IGroupLink) facade.Item).Target));
                case CatalogueItemKind.Profile:
                    return new NavTuple(new ProfilePage(),
                        ViewModelLocator.ProfileViewModel.WithModel(((IProfileLink) facade.Item).Target));
                case CatalogueItemKind.Rule:
                    return new NavTuple(new RulePage(),
                        ViewModelLocator.RuleViewModel.WithModel(((IRuleLink) facade.Item).Target));
                default:
                    return null;
            }
        }

        protected override string GetErrorString(CatalogueItemFacade parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(CatalogueItemFacade parameter) => parameter?.IsLink ?? false;
    }
}
