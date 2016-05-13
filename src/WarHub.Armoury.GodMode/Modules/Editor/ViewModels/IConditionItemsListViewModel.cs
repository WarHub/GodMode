// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;
    using Bindables;
    using Models;

    public interface IConditionItemsListViewModel : INotifyPropertyChanged
    {
        IEnumerable<IBindableGrouping<ConditionItemFacade>> ConditionItems { get; }

        ICommand CreateConditionItemCommand { get; }

        ICommand OpenConditionItemCommand { get; }
    }
}
