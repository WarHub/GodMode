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
    public class OpenCatalogueItemCommand : NavigateCommandBase<CatalogueItemFacade>
    {
        public OpenCatalogueItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, Func<IEntryLink, EntryLinkViewModel> entryLinkVmFactory,
            Func<IEntry, EntryViewModel> entryVmFactory, Func<IGroupLink, GroupLinkViewModel> groupLinkVmFactory,
            Func<IGroup, GroupViewModel> groupVmFactory, Func<IProfileLink, ProfileLinkViewModel> profileLinkVmFactory,
            Func<IProfile, ProfileViewModel> profileVmFactory, Func<IRuleLink, RuleLinkViewModel> ruleLinkVmFactory,
            Func<IRule, RuleViewModel> ruleVmFactory) : base(dependencyAggregate, navigationService)
        {
            EntryLinkVmFactory = entryLinkVmFactory;
            EntryVmFactory = entryVmFactory;
            GroupLinkVmFactory = groupLinkVmFactory;
            GroupVmFactory = groupVmFactory;
            ProfileLinkVmFactory = profileLinkVmFactory;
            ProfileVmFactory = profileVmFactory;
            RuleLinkVmFactory = ruleLinkVmFactory;
            RuleVmFactory = ruleVmFactory;
        }

        private Func<IEntryLink, EntryLinkViewModel> EntryLinkVmFactory { get; }

        private Func<IEntry, EntryViewModel> EntryVmFactory { get; }

        private Func<IGroupLink, GroupLinkViewModel> GroupLinkVmFactory { get; }

        private Func<IGroup, GroupViewModel> GroupVmFactory { get; }

        private Func<IProfileLink, ProfileLinkViewModel> ProfileLinkVmFactory { get; }

        private Func<IProfile, ProfileViewModel> ProfileVmFactory { get; }

        private Func<IRuleLink, RuleLinkViewModel> RuleLinkVmFactory { get; }

        private Func<IRule, RuleViewModel> RuleVmFactory { get; }

        protected override NavTuple GetNavTuple(CatalogueItemFacade facade)
        {
            switch (facade.ItemKind)
            {
                case CatalogueItemKind.Entry:
                    return facade.IsLink
                        ? new NavTuple(new EntryLinkPage(), EntryLinkVmFactory((IEntryLink) facade.Item))
                        : new NavTuple(new EntryPage(), EntryVmFactory((IEntry) facade.Item));
                case CatalogueItemKind.Group:
                    return facade.IsLink
                        ? new NavTuple(new GroupLinkPage(), GroupLinkVmFactory((IGroupLink) facade.Item))
                        : new NavTuple(new GroupPage(), GroupVmFactory((IGroup) facade.Item));
                case CatalogueItemKind.Profile:
                    return facade.IsLink
                        ? new NavTuple(new ProfileLinkPage(), ProfileLinkVmFactory((IProfileLink) facade.Item))
                        : new NavTuple(new ProfilePage(), ProfileVmFactory((IProfile) facade.Item));
                case CatalogueItemKind.Rule:
                    return facade.IsLink
                        ? new NavTuple(new RuleLinkPage(), RuleLinkVmFactory((IRuleLink) facade.Item))
                        : new NavTuple(new RulePage(), RuleVmFactory((IRule) facade.Item));
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
