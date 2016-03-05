// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class BindableGrouping<T> : IBindableGrouping<T>
    {
        public BindableGrouping(IEnumerable<T> items, string key = null, string shortKey = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            Key = key;
            Items = items;
            ShortKey = shortKey;
        }

        private IEnumerable<T> Items { get; }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public string Key { get; }

        public string ShortKey { get; }


#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
#pragma warning restore 0067
    }
}
