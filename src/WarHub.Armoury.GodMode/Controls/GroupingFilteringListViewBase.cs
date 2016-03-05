// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Xamarin.Forms;

    public abstract class GroupingFilteringListViewBase<T> : ListView
    {
        /// <summary>
        ///     Base items source to be filtered.
        /// </summary>
        public static readonly BindableProperty ItemsBaseSourceProperty =
            BindableProperty.Create(
                nameof(ItemsBaseSource), typeof(GroupingFilteringListViewBase<T>),
                typeof(IEnumerable<IBindableGrouping<T>>), EmptyGrouping(),
                propertyChanged: ItemsBaseSourcePropertyChanged);

        protected GroupingFilteringListViewBase()
        {
            IsGroupingEnabled = true;
            GroupDisplayBinding = new Binding(nameof(IBindableGrouping<T>.Key));
            GroupShortNameBinding = new Binding(nameof(IBindableGrouping<T>.ShortKey));
        }

        public IEnumerable<IBindableGrouping<T>> ItemsBaseSource
        {
            get { return (IEnumerable<IBindableGrouping<T>>) GetValue(ItemsBaseSourceProperty); }
            set { SetValue(ItemsBaseSourceProperty, value); }
        }

        private string LastFilterContents { get; set; }

        private static void ItemsBaseSourcePropertyChanged(BindableObject b, object oldValue, object newValue)
        {
            ((GroupingFilteringListViewBase<T>) b).OnItemsBaseSourceChanged(newValue);
        }

        private void OnItemsBaseSourceChanged(object newValue)
        {
            SetValue(ItemsSourceProperty, newValue);
            Filter(LastFilterContents);
        }

        public void Filter(string filter)
        {
            LastFilterContents = filter;
            BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                ItemsSource = ItemsBaseSource;
            }
            else
            {
                var filterInLower = filter.ToLower();
                ItemsSource =
                    ItemsBaseSource.Select(
                        grouping => grouping.GroupWhere(item => SelectKey(item).ToLower().Contains(filterInLower)))
                        .ToArray();
            }
            EndRefresh();
        }

        public abstract string SelectKey(T item);

        private static IEnumerable<IBindableGrouping<T>> EmptyGrouping()
        {
            return new[] {new BindableGrouping<T>(new T[0])};
        }
    }
}
