// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Views
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using ModelFacades;
    using Models;
    using ViewModels;
    using Xamarin.Forms;

    public partial class ConditionItemsListPage : ContentPage
    {
        public static BindableProperty ElementsProperty = BindableProperty.Create(nameof(Elements),
            typeof(IEnumerable<IBindableGrouping<ConditionItemFacade>>), typeof(ConditionItemsListPage),
            new IBindableGrouping<ConditionItemFacade>[0], propertyChanged: OnElementsPropertyChanged);

        public static BindableProperty ElementsCountProperty =
            BindableProperty.Create(nameof(ElementsCount), typeof(int), typeof(ConditionItemsListPage), default(int));

        public ConditionItemsListPage()
        {
            SetBinding(ElementsProperty,
                Binding.Create<IConditionItemsListViewModel>(viewModel => viewModel.ConditionItems));
            InitializeComponent();
        }

        public IEnumerable<IBindableGrouping<ConditionItemFacade>> Elements
        {
            get { return (IEnumerable<IBindableGrouping<ConditionItemFacade>>) GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        public int ElementsCount
        {
            get { return (int) GetValue(ElementsCountProperty); }
            set { SetValue(ElementsCountProperty, value); }
        }

        private static void OnElementsPropertyChanged(BindableObject bindable,
            object oldValue,
            object newValue)
        {
            ((ConditionItemsListPage) bindable).OnElementsPropertyChanged(
                (IEnumerable<IBindableGrouping<ConditionItemFacade>>) oldValue,
                (IEnumerable<IBindableGrouping<ConditionItemFacade>>) newValue);
        }

        private void OnElementsPropertyChanged(IEnumerable<IBindableGrouping<ConditionItemFacade>> oldValue,
            IEnumerable<IBindableGrouping<ConditionItemFacade>> newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }
            Unsubscribe(oldValue);
            Subscribe(newValue);
            UpdateElementsCount();
        }

        private void Unsubscribe(IEnumerable<IBindableGrouping<ConditionItemFacade>> oldValue)
        {
            var newNotifyCollectionChanged = oldValue as INotifyCollectionChanged;
            if (newNotifyCollectionChanged != null)
            {
                newNotifyCollectionChanged.CollectionChanged -= OnElementsCollectionChanged;
            }
            foreach (var grouping in oldValue)
            {
                grouping.CollectionChanged -= OnGroupingCollectionChanged;
            }
        }

        private void Subscribe(IEnumerable<IBindableGrouping<ConditionItemFacade>> newValue)
        {
            var newNotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (newNotifyCollectionChanged != null)
            {
                newNotifyCollectionChanged.CollectionChanged += OnElementsCollectionChanged;
            }
            foreach (var grouping in newValue)
            {
                grouping.CollectionChanged += OnGroupingCollectionChanged;
            }
        }

        private void OnGroupingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateElementsCount();
        }

        private void OnElementsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                Unsubscribe(e.OldItems.Cast<IBindableGrouping<ConditionItemFacade>>());
            }
            if (e.NewItems != null)
            {
                Subscribe(e.NewItems.Cast<IBindableGrouping<ConditionItemFacade>>());
            }
            UpdateElementsCount();
        }

        private void UpdateElementsCount()
        {
            ElementsCount = Elements?.Aggregate(0, (sum, grouping) => sum + grouping.Count()) ?? 0;
        }
    }
}
