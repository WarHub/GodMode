// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Threading.Tasks;
    using Bindables;
    using Modules.Editor.Models;
    using Mvvm.Commands;
    using Services;

    public class RemoveCatalogueItemCommand : ProgressingAsyncCommandBase<CatalogueItemFacade>
    {
        public RemoveCatalogueItemCommand(IDialogService dialogService,
            Func<IBindableMap<CatalogueItemFacade>> getMapFunc = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            DialogService = dialogService;
            GetMapFunc = getMapFunc;
        }

        private IDialogService DialogService { get; }

        private Func<IBindableMap<CatalogueItemFacade>> GetMapFunc { get; }

        public RemoveCatalogueItemCommand For(Func<IBindableMap<CatalogueItemFacade>> getMapFunc)
        {
            return new RemoveCatalogueItemCommand(DialogService, getMapFunc);
        }

        protected override async Task ExecuteCoreAsync(CatalogueItemFacade parameter)
        {
            var bindableMap = GetMapFunc?.Invoke();
            if (bindableMap == null || parameter == null)
            {
                await InformRequestCannotBeProcessedAsync();
                return;
            }
            //TODO check dependencies
            var result = await GetPermissionAsync();
            if (result)
            {
                bindableMap.Remove(parameter);
            }
        }

        private async Task InformRequestCannotBeProcessedAsync()
        {
            await
                DialogService.ShowDialogAsync("Ooops",
                    "Something's wrong: the request cannot be processed." +
                    " Additional info: the command was not set up properly.",
                    "ok");
        }

        private async Task<bool> GetPermissionAsync()
        {
            return await
                DialogService.ShowDialogAsync("Warning",
                    "Currently there are no checks performed, whether there are any dependencies on this item. Proceed?",
                    "yes", "no");
        }

        protected override bool CanExecuteCore(CatalogueItemFacade parameter) => parameter != null && GetMapFunc != null;
    }
}
