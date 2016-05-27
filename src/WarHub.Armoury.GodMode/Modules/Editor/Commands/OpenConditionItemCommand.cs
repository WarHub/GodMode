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
    public class OpenConditionItemCommand : NavigateCommandBase<ConditionItemFacade>
    {
        public OpenConditionItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService,
            Func<ICatalogueCondition, CatalogueConditionViewModel> conditionVmFactory,
            Func<IConditionGroup, CatalogueConditionGroupViewModel> groupVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            ConditionVmFactory = conditionVmFactory;
            GroupVmFactory = groupVmFactory;
        }

        private Func<ICatalogueCondition, CatalogueConditionViewModel> ConditionVmFactory { get; }

        private Func<IConditionGroup, CatalogueConditionGroupViewModel> GroupVmFactory { get; }

        protected override NavTuple GetNavTuple(ConditionItemFacade parameter)
        {
            switch (parameter.ItemKind)
            {
                case ConditionItemKind.Condition:
                    return new NavTuple(new CatalogueConditionPage(),
                        ConditionVmFactory((ICatalogueCondition) parameter.Item));
                case ConditionItemKind.Group:
                    return new NavTuple(new CatalogueConditionGroupPage(),
                        GroupVmFactory((ICatalogueConditionGroup) parameter.Item));
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
