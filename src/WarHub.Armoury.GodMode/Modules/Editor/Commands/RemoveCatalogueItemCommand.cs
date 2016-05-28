// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Bindables;
    using GodMode.Commands;
    using Models;
    
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RemoveCatalogueItemCommand : AppAsyncCommandBase<CatalogueItemFacade>
    {
        public RemoveCatalogueItemCommand(IAppCommandDependencyAggregate dependencyAggregate,
            IBindableMap<CatalogueItemFacade> facades) : base(dependencyAggregate)
        {
            Facades = facades;
        }

        private IBindableMap<CatalogueItemFacade> Facades { get; }

        protected override async Task ExecuteCoreAsync(CatalogueItemFacade parameter)
        {
            var isPermission = await GetPermissionAsync();
            if (isPermission)
            {
                Facades.Remove(parameter);
            }
        }

        private async Task<bool> GetPermissionAsync()
        {
            return await
                DialogService.ShowDialogAsync("Warning",
                    "Currently there are no checks performed, whether there are any dependencies on this item. Proceed?",
                    "yes", "no");
        }

        protected override bool CanExecuteCore(CatalogueItemFacade parameter) => parameter != null;
    }
}
