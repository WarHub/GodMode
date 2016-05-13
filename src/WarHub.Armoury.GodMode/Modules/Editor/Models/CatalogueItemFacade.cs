// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using System;
    using System.ComponentModel;

    public class CatalogueItemFacade : ModelFacadeBase
    {
        public CatalogueItemFacade(object item, CatalogueItemKind itemKind, Func<string> getName,
            Func<string> getDetail = null, bool isLink = false, bool isShared = false)
        {
            GetName = getName;
            ItemKind = itemKind;
            GetDetail = getDetail;
            Item = item;
            IsLink = isLink;
            IsShared = isShared;
        }

        public override string Detail => GetDetail?.Invoke();

        public override object Model => Item;

        public override string Name => GetName?.Invoke() ?? "(no name)";

        public bool IsLink { get; }

        public bool IsShared { get; }

        public object Item { get; }

        public CatalogueItemKind ItemKind { get; }

        private Func<string> GetDetail { get; }

        private Func<string> GetName { get; }

        protected override void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnModelPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Name))
            {
                RaisePropertyChanged(nameof(Name));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
