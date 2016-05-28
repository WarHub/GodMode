// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using Bindables;
    using Model;
    using Mvvm.Commands;

    public class ConditionItemFacade : ModelFacadeBase
    {
        public ConditionItemFacade(ICatalogueCondition condition, ICommand<ConditionItemFacade> removeCommand)
        {
            Item = condition;
            RemoveCommand = removeCommand?.SetParameter(this);
            ItemKind = ConditionItemKind.Condition;
            GetNameFunc = condition.Stringify;
            GetDetailFunc = null;
        }

        public ConditionItemFacade(ICatalogueConditionGroup group, ICommand<ConditionItemFacade> removeCommand)
        {
            Item = group;
            RemoveCommand = removeCommand?.SetParameter(this);
            ItemKind = ConditionItemKind.Group;
            GetNameFunc = group.Type.ToString;
            GetDetailFunc = group.ToDetailString;
        }

        public override string Detail => GetDetailFunc?.Invoke();

        public override object Model => Item;

        public override string Name => GetNameFunc?.Invoke();

        public override ICommand RemoveCommand { get; }

        public object Item { get; }

        public ConditionItemKind ItemKind { get; }

        private Func<string> GetDetailFunc { get; }

        private Func<string> GetNameFunc { get; }

        public override string ToString()
        {
            return Name;
        }

        protected override void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnModelPropertyChanged(sender, e);
            if (e.PropertyName != nameof(ICatalogueItem.Context))
            {
                RaisePropertyChanged(nameof(Name));
            }
        }

        protected override void OnPropertyChangedSubscribed()
        {
            base.OnPropertyChangedSubscribed();
            if (ItemKind == ConditionItemKind.Condition)
            {
                return;
            }
            var container = (ICatalogueConditionNodeContainer) Item;
            container.Conditions.PropertyChanged += OnModelCollectionPropertyChanged;
            container.ConditionGroups.PropertyChanged += OnModelCollectionPropertyChanged;
        }

        protected override void OnPropertyChangedUnsubscribed()
        {
            base.OnPropertyChangedUnsubscribed();
            if (ItemKind == ConditionItemKind.Condition)
            {
                return;
            }
            var container = (ICatalogueConditionNodeContainer) Item;
            container.Conditions.PropertyChanged -= OnModelCollectionPropertyChanged;
            container.ConditionGroups.PropertyChanged -= OnModelCollectionPropertyChanged;
        }

        private void OnModelCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ICollection.Count))
            {
                RaisePropertyChanged(nameof(Detail));
            }
        }
    }
}
