// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Threading.Tasks;
    using Model;
    using Modules.Editor.Models;
    using Mvvm.Commands;
    using Services;

    public class CreateModifierCommand : ProgressingAsyncCommandBase
    {
        public CreateModifierCommand(IDialogService dialogService, OpenModifierCommand openModifierCommand,
            Func<ModifierFacade> create = null)
        {
            DialogService = dialogService;
            OpenModifierCommand = openModifierCommand;
            Create = create;
        }

        private Func<ModifierFacade> Create { get; }

        private IDialogService DialogService { get; }

        private OpenModifierCommand OpenModifierCommand { get; }

        public CreateModifierCommand EnableFor(INodeSimple<IEntryModifier> node)
        {
            return new CreateModifierCommand(DialogService, OpenModifierCommand, () => CreateFacade(node));
        }

        public CreateModifierCommand EnableFor(INodeSimple<IGroupModifier> node)
        {
            return new CreateModifierCommand(DialogService, OpenModifierCommand, () => CreateFacade(node));
        }

        public CreateModifierCommand EnableFor(INodeSimple<IProfileModifier> node)
        {
            return new CreateModifierCommand(DialogService, OpenModifierCommand, () => CreateFacade(node));
        }

        public CreateModifierCommand EnableFor(INodeSimple<IRuleModifier> node)
        {
            return new CreateModifierCommand(DialogService, OpenModifierCommand, () => CreateFacade(node));
        }

        protected override async Task ExecuteCoreAsync()
        {
            if (Create == null)
            {
                await InformRequestCannotBeProcessedAsync();
                return;
            }
            var modifier = Create();
            if (modifier == null)
            {
                //inform
                return;
            }
            if (OpenModifierCommand.CanExecute(modifier))
            {
                OpenModifierCommand.Execute(modifier);
                return;
            }
            await
                DialogService.ShowDialogAsync("Cannot navigate", "There was an error and the item could not be opened.",
                    "cancel");
        }

        protected override bool CanExecuteCore() => Create != null;

        private async Task InformRequestCannotBeProcessedAsync()
        {
            await
                DialogService.ShowDialogAsync("Ooops",
                    "Something's wrong: the request cannot be processed." +
                    " Additional info: the command was not set up properly.",
                    "ok");
        }

        private static ModifierFacade CreateFacade(INodeSimple<IEntryModifier> node)
        {
            return node.AddNew().ToFacade();
        }

        private static ModifierFacade CreateFacade(INodeSimple<IGroupModifier> node)
        {
            return node.AddNew().ToFacade();
        }

        private static ModifierFacade CreateFacade(INodeSimple<IProfileModifier> node)
        {
            return node.AddNew().ToFacade();
        }

        private static ModifierFacade CreateFacade(INodeSimple<IRuleModifier> node)
        {
            return node.AddNew().ToFacade();
        }
    }
}
