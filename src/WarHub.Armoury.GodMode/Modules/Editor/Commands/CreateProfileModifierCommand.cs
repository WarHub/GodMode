﻿// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using Model;
    using Models;

    public delegate CreateProfileModifierCommand CreateProfileModifierCommandFactory(
        INodeSimple<IProfileModifier> modifiers);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateProfileModifierCommand : AppAsyncCommandBase
    {
        public CreateProfileModifierCommand(IAppCommandDependencyAggregate dependencyAggregate,
            OpenModifierCommand openModifierCommand, INodeSimple<IProfileModifier> modifiers)
            : base(dependencyAggregate)
        {
            OpenModifierCommand = openModifierCommand;
            Modifiers = modifiers;
        }

        private INodeSimple<IProfileModifier> Modifiers { get; }

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
