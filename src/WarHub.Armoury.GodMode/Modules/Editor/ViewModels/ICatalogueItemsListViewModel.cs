// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Bindables;
    using Commands;
    using Models;

    public interface ICatalogueItemsListViewModel : INotifyPropertyChanged
    {
        IEnumerable<IBindableGrouping<CatalogueItemFacade>> CatalogueItems { get; }

        CreateCatalogueItemCommandBase CreateCatalogueItemCommand { get; }

        OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }
    }
}
