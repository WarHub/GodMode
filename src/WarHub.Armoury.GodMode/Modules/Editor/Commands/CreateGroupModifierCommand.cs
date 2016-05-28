// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model;
    using Models;

    public delegate CreateGroupModifierCommand CreateGroupModifierCommandFactory(INodeSimple<IGroupModifier> modifiers);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateGroupModifierCommand : AppAsyncCommandBase
    {
        public CreateGroupModifierCommand(IAppCommandDependencyAggregate dependencyAggregate,
            OpenModifierCommand openModifierCommand, INodeSimple<IGroupModifier> modifiers)
            : base(dependencyAggregate)
        {
            OpenModifierCommand = openModifierCommand;
            Modifiers = modifiers;
        }

        private INodeSimple<IGroupModifier> Modifiers { get; }

        private OpenModifierCommand OpenModifierCommand { get; }

        protected override async Task ExecuteCoreAsync()
        {
            var modifier = Modifiers.AddNew().ToFacade(null);
            if (OpenModifierCommand.CanExecute(modifier))
            {
                OpenModifierCommand.Execute(modifier);
                return;
            }
            await
                DialogService.ShowDialogAsync("Cannot navigate", "There was an error and the item could not be opened.",
                    "cancel");
        }
    }
}
