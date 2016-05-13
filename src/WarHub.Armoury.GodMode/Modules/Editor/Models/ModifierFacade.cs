// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Bindables;
    using Model;

    public class ModifierFacade : ModelFacadeBase
    {
        private ModifierFacade(ICatalogueConditionNodeContainer modifier, ModifierKind kind, Func<string> getNameFunc)
        {
            Modifier = modifier;
            Kind = kind;
            GetNameFunc = getNameFunc;
            GetDetailFunc = modifier.ToDetailString;
        }

        public ModifierFacade(IEntryModifier modifier) : this(modifier, ModifierKind.Entry, modifier.Stringify)
        {
        }

        public ModifierFacade(IGroupModifier modifier) : this(modifier, ModifierKind.Group, modifier.Stringify)
        {
        }

        public ModifierFacade(IProfileModifier modifier) : this(modifier, ModifierKind.Profile, modifier.Stringify)
        {
        }

        public ModifierFacade(IRuleModifier modifier) : this(modifier, ModifierKind.Rule, modifier.Stringify)
        {
        }

        public override string Detail => GetDetailFunc?.Invoke();

        public override object Model => Modifier;

        public override string Name => GetNameFunc?.Invoke();

        public ModifierKind Kind { get; }

        public ICatalogueConditionNodeContainer Modifier { get; }

        private IEnumerable<string> DetailDependencies
        {
            get { yield return nameof(ICollection.Count); }
        }

        private Func<string> GetDetailFunc { get; }

        private Func<string> GetNameFunc { get; }

        private IEnumerable<string> NameDependencies
        {
            get
            {
                yield return nameof(IEntryModifier.Value);
                yield return nameof(IEntryModifier.Action);
                yield return nameof(IEntryModifier.Field);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        protected override void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnModelPropertyChanged(sender, e);
            if (NameDependencies.Any(s => s == e.PropertyName))
            {
                RaisePropertyChanged(nameof(Name));
            }
        }

        protected override void OnPropertyChangedSubscribed()
        {
            base.OnPropertyChangedSubscribed();
            Modifier.Conditions.PropertyChanged += OnModelCollectionPropertyChanged;
        }

        protected override void OnPropertyChangedUnsubscribed()
        {
            base.OnPropertyChangedUnsubscribed();
            Modifier.Conditions.PropertyChanged -= OnModelCollectionPropertyChanged;
        }

        private void OnModelCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (DetailDependencies.Any(s => s == e.PropertyName))
            {
                RaisePropertyChanged(nameof(Detail));
            }
        }
    }
}
