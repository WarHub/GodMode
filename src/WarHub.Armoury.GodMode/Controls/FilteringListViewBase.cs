// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;

    public abstract class FilteringListViewBase<T> : ListView
    {
        /// <summary>
        ///     Base items source to be filtered.
        /// </summary>
        public static readonly BindableProperty ItemsBaseSourceProperty =
            BindableProperty.Create(nameof(ItemsBaseSource), typeof(IEnumerable<T>), typeof(FilteringListViewBase<T>),
                new T[0], propertyChanged: ItemsBaseSourcePropertyChanged);

        public IEnumerable<T> ItemsBaseSource
        {
            get { return (IEnumerable<T>) GetValue(ItemsBaseSourceProperty); }
            set { SetValue(ItemsBaseSourceProperty, value); }
        }

        private static void ItemsBaseSourcePropertyChanged(BindableObject bindable, object value, object newValue)
        {
            bindable.SetValue(ItemsSourceProperty, newValue);
        }


        public void Filter(string filter)
        {
            BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                ItemsSource = ItemsBaseSource;
            }
            else
            {
                var filterInLower = filter.ToLower();
                ItemsSource = ItemsBaseSource.Where(x => SelectKey(x).ToLower().Contains(filterInLower));
            }
            EndRefresh();
        }

        public abstract string SelectKey(T item);
    }
}
