// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AppServices;
    using GodMode.Commands;
    using Model;
    using Models;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class OpenLinkTargetAsChildCommand : NavigateCommandBase<CatalogueItemFacade>
    {
        public OpenLinkTargetAsChildCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<IEntry, EntryViewModel> entryVmFactory,
            Func<IGroup, GroupViewModel> groupVmFactory, Func<IProfile, ProfileViewModel> profileVmFactory,
            Func<IRule, RuleViewModel> ruleVmFactory) : base(dependencyAggregate, navigationService)
        {
            EntryVmFactory = entryVmFactory;
            GroupVmFactory = groupVmFactory;
            ProfileVmFactory = profileVmFactory;
            RuleVmFactory = ruleVmFactory;
        }

        private Func<IEntry, EntryViewModel> EntryVmFactory { get; }

        private Func<IGroup, GroupViewModel> GroupVmFactory { get; }

        private Func<IProfile, ProfileViewModel> ProfileVmFactory { get; }

        private Func<IRule, RuleViewModel> RuleVmFactory { get; }

        protected override NavTuple GetNavTuple(CatalogueItemFacade facade)
        {
            switch (facade.ItemKind)
            {
                case CatalogueItemKind.Entry:
                    return new NavTuple(new EntryPage(), EntryVmFactory(((IEntryLink) facade.Item).Target));
                case CatalogueItemKind.Group:
                    return new NavTuple(new GroupPage(), GroupVmFactory(((IGroupLink) facade.Item).Target));
                case CatalogueItemKind.Profile:
                    return new NavTuple(new ProfilePage(), ProfileVmFactory(((IProfileLink) facade.Item).Target));
                case CatalogueItemKind.Rule:
                    return new NavTuple(new RulePage(), RuleVmFactory(((IRuleLink) facade.Item).Target));
                default:
                    return null;
            }
        }

        protected override string GetErrorString(CatalogueItemFacade parameter)
        {
            return $"Currently there is no implementation to open '{parameter?.Name ?? "'null'"}'";
        }

        protected override bool CanExecuteCore(CatalogueItemFacade parameter) => parameter?.IsLink ?? false;
    }
}
