// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Views
{
    using System.Collections.Specialized;
    using System.Linq;
    using Bindables;
    using Models;
    using ViewModels;
    using Xamarin.Forms;

    public partial class ModifiersListPage : ContentPage
    {
        public static BindableProperty ModifiersProperty = BindableProperty.Create(nameof(Modifiers),
            typeof(IBindableGrouping<ModifierFacade>), typeof(ModifiersListPage),
            new BindableGrouping<ModifierFacade>(new ModifierFacade[0]), propertyChanged: OnModifiersPropertyChanged);

        public static BindableProperty ModifiersCountProperty =
            BindableProperty.Create(nameof(ModifiersCount), typeof(int), typeof(ModifiersListPage), default(int));

        public ModifiersListPage()
        {
            SetBinding(ModifiersProperty, Binding.Create<IModifiersListViewModel>(viewModel => viewModel.Modifiers));
            InitializeComponent();
        }

        public IBindableGrouping<ModifierFacade> Modifiers
        {
            get { return (IBindableGrouping<ModifierFacade>) GetValue(ModifiersProperty); }
            set { SetValue(ModifiersProperty, value); }
        }

        public int ModifiersCount
        {
            get { return (int) GetValue(ModifiersCountProperty); }
            set { SetValue(ModifiersCountProperty, value); }
        }

        private static void OnModifiersPropertyChanged(BindableObject bindable,
            object oldValue,
            object newValue)
        {
            ((ModifiersListPage) bindable).OnModifiersPropertyChanged((INotifyCollectionChanged) oldValue,
                (INotifyCollectionChanged) newValue);
        }

        private void OnModifiersPropertyChanged(INotifyCollectionChanged oldValue,
            INotifyCollectionChanged newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }
            Unsubscribe(oldValue);
            Subscribe(newValue);
            UpdateModifiersCount();
        }

        private void Unsubscribe(INotifyCollectionChanged oldValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= OnModifiersCollectionChanged;
            }
        }

        private void Subscribe(INotifyCollectionChanged newValue)
        {
            if (newValue != null)
            {
                newValue.CollectionChanged += OnModifiersCollectionChanged;
            }
        }

        private void OnModifiersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateModifiersCount();
        }

        private void UpdateModifiersCount()
        {
            ModifiersCount = Modifiers?.Count() ?? 0;
        }
    }
}
