// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using Model;
    using Modules.Editor.Models;
    using Modules.Editor.Views;
    using Services;

    public class OpenConditionItemCommand : NavigateCommandBase<ConditionItemFacade>
    {
        public OpenConditionItemCommand(IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
        }

        protected override NavTuple GetNavTuple(ConditionItemFacade parameter)
        {
            switch (parameter.ItemKind)
            {
                case ConditionItemKind.Condition:
                    return new NavTuple(new CatalogueConditionPage(),
                        ViewModelLocator.CatalogueConditionViewModel.WithModel((ICatalogueCondition) parameter.Item));
                case ConditionItemKind.Group:
                    return new NavTuple(new CatalogueConditionGroupPage(),
                        ViewModelLocator.CatalogueConditionGroupViewModel.WithModel(
                            (ICatalogueConditionGroup) parameter.Item));
                default:
                    return null;
            }
        }

        protected override string GetErrorString(ConditionItemFacade parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(ConditionItemFacade parameter) => parameter != null;
    }
}
