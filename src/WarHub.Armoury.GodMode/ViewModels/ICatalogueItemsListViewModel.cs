// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;
    using ModelFacades;
    using Models;

    public interface ICatalogueItemsListViewModel : INotifyPropertyChanged
    {
        IEnumerable<IBindableGrouping<CatalogueItemFacade>> CatalogueItems { get; }

        ICommand CreateCatalogueItemCommand { get; }

        ICommand OpenCatalogueItemCommand { get; }
    }
}
