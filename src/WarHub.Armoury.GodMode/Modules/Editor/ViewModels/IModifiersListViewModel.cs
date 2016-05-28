// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.ComponentModel;
    using Bindables;
    using Commands;
    using Models;
    using Mvvm.Commands;

    public interface IModifiersListViewModel : INotifyPropertyChanged
    {
        ICommand CreateModifierCommand { get; }

        IBindableGrouping<ModifierFacade> Modifiers { get; }

        OpenModifierCommand OpenModifierCommand { get; }
    }
}
