// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Bindables
{
    using System.Collections.Generic;
    using Mvvm.Commands;

    public interface IBindableMap<T> : IBindableGrouping<T>, ICollection<T>
    {
        ICommand<T> RemoveCommand { get; }
    }
}
