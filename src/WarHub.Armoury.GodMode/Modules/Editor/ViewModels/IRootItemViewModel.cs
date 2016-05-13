// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Model;

    public interface IRootItemViewModel : INotifyPropertyChanged
    {
        IEnumerable<ICategory> Categories { get; }

        ICategory Category { get; set; }

        bool IsRootItem { get; }
    }
}
