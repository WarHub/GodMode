// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using Bindables;
    using GodMode.Commands;
    using Models;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoveConditionItemCommand : AppCommandBase<ConditionItemFacade>
    {
        public RemoveConditionItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IBindableMap<ConditionItemFacade> facades) : base(dependencyAggregate)
        {
            Facades = facades;
        }

        private IBindableMap<ConditionItemFacade> Facades { get; }

        protected override void ExecuteCore(ConditionItemFacade parameter)
        {
            Facades.Remove(parameter);
        }

        protected override bool CanExecuteCore(ConditionItemFacade parameter) => parameter != null;
    }
}
