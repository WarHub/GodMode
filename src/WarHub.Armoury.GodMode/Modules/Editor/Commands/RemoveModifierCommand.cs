// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using Bindables;
    using GodMode.Commands;
    using Models;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoveModifierCommand : AppCommandBase<ModifierFacade>
    {
        public RemoveModifierCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IBindableMap<ModifierFacade> facades) : base(dependencyAggregate)
        {
            Facades = facades;
        }

        private IBindableMap<ModifierFacade> Facades { get; }

        protected override void ExecuteCore(ModifierFacade parameter)
        {
            Facades.Remove(parameter);
        }

        protected override bool CanExecuteCore(ModifierFacade parameter) => parameter != null;
    }
}
