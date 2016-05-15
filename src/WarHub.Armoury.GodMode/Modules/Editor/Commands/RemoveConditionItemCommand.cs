// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using System.Threading.Tasks;
    using AppServices;
    using Bindables;
    using Models;
    using Mvvm.Commands;

    public class RemoveConditionItemCommand : ProgressingAsyncCommandBase<ConditionItemFacade>
    {
        public RemoveConditionItemCommand(IDialogService dialogService,
            Func<IBindableMap<ConditionItemFacade>> getMapFunc = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            DialogService = dialogService;
            GetMapFunc = getMapFunc;
        }

        private IDialogService DialogService { get; }

        private Func<IBindableMap<ConditionItemFacade>> GetMapFunc { get; }

        public RemoveConditionItemCommand For(Func<IBindableMap<ConditionItemFacade>> getMapFunc)
        {
            return new RemoveConditionItemCommand(DialogService, getMapFunc);
        }

        protected override async Task ExecuteCoreAsync(ConditionItemFacade parameter)
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

        protected override bool CanExecuteCore(ConditionItemFacade parameter) => parameter != null && GetMapFunc != null;
    }
}
