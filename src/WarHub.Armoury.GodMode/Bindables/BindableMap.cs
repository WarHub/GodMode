// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Bindables
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using Mvvm;

    public class BindableMap<T, TSource> : IBindableMap<T>
    {
        private readonly object _objectLock = new object();

        public BindableMap(IList<TSource> itemsSource, Func<TSource, T> map, Func<T, TSource> reverseMap,
            string key = null)
        {
            if (itemsSource == null)
                throw new ArgumentNullException(nameof(itemsSource));
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (reverseMap == null)
                throw new ArgumentNullException(nameof(reverseMap));
            ItemsSource = itemsSource;
            Map = map;
            ReverseMap = reverseMap;
            Key = key;
            ShortKey = key.ShortenKey();
            Items = new ObservableCollection<T>(itemsSource.Select(map));
            ((INotifyPropertyChanged) Items).PropertyChanged += OnItemsPropertyChanged;
            Items.CollectionChanged += OnItemsCollectionChanged;
        }

        private bool IsChanging { get; set; }

        private ObservableCollection<T> Items { get; }

        private IList<TSource> ItemsSource { get; }

        private Func<TSource, T> Map { get; }

        private Func<T, TSource> ReverseMap { get; }

        public string ShortKey { get; }

        public string Key { get; }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                lock (_objectLock)
                {
                    if (CollectionChangedCore == null)
                    {
                        OnCollectionChangedSubscribed();
                    }
                    CollectionChangedCore += value;
                }
            }
            remove
            {
                lock (_objectLock)
                {
                    CollectionChangedCore -= value;
                    if (CollectionChangedCore == null)
                    {
                        OnCollectionChangeUnsubscribed();
                    }
                }
            }
        }

        public void Add(T item)
        {
            using (BeginChanging())
            {
                ItemsSource.Add(ReverseMap(item));
                Items.Add(item);
            }
        }

        public void Clear()
        {
            using (BeginChanging())
            {
                ItemsSource.Clear();
                Items.Clear();
            }
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            using (BeginChanging())
            {
                return ItemsSource.Remove(ReverseMap(item)) && Items.Remove(item);
            }
        }

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChangedCore?.Invoke(this, e);
        }

        private void OnItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private event NotifyCollectionChangedEventHandler CollectionChangedCore;

        private void OnCollectionChangedSubscribed()
        {
            var itemsSourceNotifying = ItemsSource as INotifyCollectionChanged;
            if (itemsSourceNotifying != null)
            {
                itemsSourceNotifying.CollectionChanged += OnItemsSourceCollectionChanged;
            }
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsChanging)
            {
                // source changed from this Map's call
                return;
            }
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count == 1)
            {
                var newItem = Map((TSource) e.NewItems[0]);
                if (e.NewStartingIndex < 0)
                {
                    Items.Add(newItem);
                }
                else
                {
                    Items.Insert(e.NewStartingIndex, newItem);
                }
                return;
            }
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Count == 1)
            {
                if (e.OldStartingIndex < 0)
                {
                    Items.Remove(Map((TSource) e.OldItems[0]));
                }
                else
                {
                    Items.RemoveAt(e.OldStartingIndex);
                }
                return;
            }
            if (e.Action == NotifyCollectionChangedAction.Move && e.NewItems.Count == 1)
            {
                Items.Move(e.OldStartingIndex, e.NewStartingIndex);
                return;
            }
            Items.Clear();
            foreach (var item in ItemsSource)
            {
                Items.Add(Map(item));
            }
        }

        private DisposeActionExecutor BeginChanging()
        {
            IsChanging = true;
            return new DisposeActionExecutor(EndChanging);
        }

        private void EndChanging()
        {
            IsChanging = false;
        }

        private void OnCollectionChangeUnsubscribed()
        {
            var itemsSourceNotifying = ItemsSource as INotifyCollectionChanged;
            if (itemsSourceNotifying != null)
            {
                itemsSourceNotifying.CollectionChanged -= OnItemsSourceCollectionChanged;
            }
        }
    }
}
