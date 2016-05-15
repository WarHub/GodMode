// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Threading.Tasks;
    using Bindables;
    using Models;
    using Mvvm.Commands;
    using Services;

    public class RemoveModifierCommand : ProgressingAsyncCommandBase<ModifierFacade>
    {
        public RemoveModifierCommand(IDialogService dialogService, Func<IBindableMap<ModifierFacade>> getMapFunc = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            DialogService = dialogService;
            GetMapFunc = getMapFunc;
        }

        private IDialogService DialogService { get; }

        private Func<IBindableMap<ModifierFacade>> GetMapFunc { get; }

        public RemoveModifierCommand For(Func<IBindableMap<ModifierFacade>> getMapFunc)
        {
            return new RemoveModifierCommand(DialogService, getMapFunc);
        }

        protected override async Task ExecuteCoreAsync(ModifierFacade parameter)
        {
            var bindableMap = GetMapFunc?.Invoke();
            if (bindableMap == null || parameter == null)
            {
                await InformRequestCannotBeProcessedAsync();
                return;
            }
            bindableMap.Remove(parameter);
        }

        private async Task InformRequestCannotBeProcessedAsync()
        {
            await
                DialogService.ShowDialogAsync("Ooops",
                    "Something's wrong: the request cannot be processed." +
                    " Additional info: the command was not set up properly.",
                    "ok");
        }

        protected override bool CanExecuteCore(ModifierFacade parameter) => parameter != null && GetMapFunc != null;
    }
}
