// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Models
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    public interface IBindableGrouping<out T> : IGrouping<string, T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        string ShortKey { get; }
    }
}
