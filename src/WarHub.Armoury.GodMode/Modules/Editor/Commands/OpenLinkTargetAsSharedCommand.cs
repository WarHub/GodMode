// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Services;
    using ViewModels;

    public class OpenLinkTargetAsSharedCommand : OpenLinkTargetAsChildCommand
    {
        public OpenLinkTargetAsSharedCommand(IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
        }

        protected override async Task ExecuteCoreAsync(CatalogueItemFacade parameter)
        {
            var navTuple = GetNavTuple(parameter);
            if (navTuple == null)
            {
                await DialogService.ShowDialogAsync("View unavailable", GetErrorString(parameter), "Oh well");
                return;
            }
            while (true)
            {
                var currentPage = NavigationService.NavigationStack.Last();
                if (currentPage.BindingContext.GetType() == typeof(CatalogueViewModel))
                {
                    break;
                }
                await NavigationService.PopAsync(false);
            }
            await NavigationService.NavigateAsync(navTuple.Page, navTuple.ViewModel);
        }
    }
}
