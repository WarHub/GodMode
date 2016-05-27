// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using AppServices;
    using GodMode.Commands;
    using Model;
    using ViewModels;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class OpenLinkTargetAsSharedCommand : OpenLinkTargetAsChildCommand
    {
        public OpenLinkTargetAsSharedCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<IEntry, EntryViewModel> entryVmFactory,
            Func<IGroup, GroupViewModel> groupVmFactory, Func<IProfile, ProfileViewModel> profileVmFactory,
            Func<IRule, RuleViewModel> ruleVmFactory)
            : base(
                dependencyAggregate, navigationService, entryVmFactory, groupVmFactory, profileVmFactory, ruleVmFactory)
        {
        }

        protected override async Task NavigateAsync(NavTuple navTuple)
        {
            while (NavigationService.NavigationStack.Count > 0)
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
