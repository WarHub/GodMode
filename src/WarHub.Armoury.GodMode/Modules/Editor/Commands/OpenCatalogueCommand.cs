// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Commands
{
    using System;
    using GodMode.Commands;
    using Model.Repo;
    using Services;

    public class OpenCatalogueCommand : NavigateCommandBase<CatalogueInfo>
    {
        public OpenCatalogueCommand(IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
        }

        protected override NavTuple GetNavTuple(CatalogueInfo parameter)
        {
            throw new NotImplementedException();
        }

        protected override string GetErrorString(CatalogueInfo parameter)
        {
            return $"Currently there is no implementation to open '{parameter.Name}'";
        }

        protected override bool CanExecuteCore(CatalogueInfo parameter) => parameter != null;
    }
}
