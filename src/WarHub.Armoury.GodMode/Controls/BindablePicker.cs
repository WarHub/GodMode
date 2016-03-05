// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;
    using Mvvm;
    using Xamarin.Forms;

    public class BindablePicker : Picker
    {
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IEnumerable), typeof(BindablePicker), default(IEnumerable), propertyChanged: OnItemsSourceChanged);

        public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
            typeof(object), typeof(BindablePicker), default(object), BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);

        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public string DisplayMember { get; set; }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private bool IsItemsSourceChanging { get; set; }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (IsItemsSourceChanging)
            {
                return;
            }
            SelectedItem = SelectedIndex < 0 || SelectedIndex > Items.Count - 1
                ? null
                : ItemsSource.Cast<object>().Skip(SelectedIndex).First();
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = bindable as BindablePicker;
            picker?.OnSelectedItemChangedCore(oldValue, newValue);
        }

        private void OnSelectedItemChangedCore(object oldValue, object newValue)
        {
            SelectedIndex = newValue == null || ItemsSource == null
                ? -1
                : ItemsSource.Cast<object>().ToList().IndexOf(newValue);
        }

        private string GetDisplayString(object item)
        {
            return string.IsNullOrWhiteSpace(DisplayMember)
                ? item.ToString()
                : item.GetType().GetRuntimeProperty(DisplayMember).GetValue(item).ToString();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = bindable as BindablePicker;
            if (picker == null)
            {
                return;
            }
            picker.Unsubscribe(oldValue);
            picker.SetItems((IEnumerable) newValue);
            picker.Subscribe(newValue);
        }

        private void Unsubscribe(object oldValue)
        {
            var notifyCollectionChanged = oldValue as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
            }
        }

        private void Subscribe(object newValue)
        {
            var notifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged += OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetItems(ItemsSource);
        }

        private void SetItems(IEnumerable newItems)
        {
            var containsSelected = false;
            var selectedItem = SelectedItem;
            using (BeginChangingItemsSource())
            {
                Items.Clear();
                if (newItems == null)
                {
                    return;
                }
                foreach (var item in newItems)
                {
                    containsSelected |= item == selectedItem;
                    Items.Add(GetDisplayString(item));
                }
                if (!containsSelected)
                {
                    SelectedItem = null;
                }
            }
        }

        private DisposeActionExecutor BeginChangingItemsSource()
        {
            IsItemsSourceChanging = true;
            return new DisposeActionExecutor(() => IsItemsSourceChanging = false);
        }
    }
}
