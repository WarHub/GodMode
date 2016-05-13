// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using Model;
    using Modules.Editor.Models;
    using Modules.Editor.Views;
    using Services;

    public class OpenCatalogueItemCommand : NavigateCommandBase<CatalogueItemFacade>
    {
        public OpenCatalogueItemCommand(INavigationService navigationService, IDialogService dialogService)
            : base(dialogService, navigationService)
        {
        }

        protected override NavTuple GetNavTuple(CatalogueItemFacade facade)
        {
            switch (facade.ItemKind)
            {
                case CatalogueItemKind.Entry:
                    return facade.IsLink
                        ? new NavTuple(new EntryLinkPage(),
                            ViewModelLocator.EntryLinkViewModel.WithModel((IEntryLink) facade.Item))
                        : new NavTuple(new EntryPage(), ViewModelLocator.EntryViewModel.WithModel((IEntry) facade.Item));
                case CatalogueItemKind.Group:
                    return facade.IsLink
                        ? new NavTuple(new GroupLinkPage(),
                            ViewModelLocator.GroupLinkViewModel.WithModel((IGroupLink) facade.Item))
                        : new NavTuple(new GroupPage(), ViewModelLocator.GroupViewModel.WithModel((IGroup) facade.Item));
                case CatalogueItemKind.Profile:
                    return facade.IsLink
                        ? new NavTuple(new ProfileLinkPage(),
                            ViewModelLocator.ProfileLinkViewModel.WithModel((IProfileLink) facade.Item))
                        : new NavTuple(new ProfilePage(),
                            ViewModelLocator.ProfileViewModel.WithModel((IProfile) facade.Item));
                case CatalogueItemKind.Rule:
                    return facade.IsLink
                        ? new NavTuple(new RuleLinkPage(),
                            ViewModelLocator.RuleLinkViewModel.WithModel((IRuleLink) facade.Item))
                        : new NavTuple(new RulePage(), ViewModelLocator.RuleViewModel.WithModel((IRule) facade.Item));
                default:
                    return null;
            }
        }

        protected override string GetErrorString(CatalogueItemFacade parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(CatalogueItemFacade parameter) => parameter != null;
    }
}
