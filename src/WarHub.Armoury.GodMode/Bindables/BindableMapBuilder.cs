// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Bindables
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Modules.Editor.Commands;
    using Modules.Editor.Models;

    public class BindableMapBuilder
    {
        public BindableMapBuilder(
            Func<IBindableMap<CatalogueItemFacade>, RemoveCatalogueItemCommand> removeCatalogueItemCommandFactory,
            Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> removeConditionItemCommandFactory,
            Func<IBindableMap<ModifierFacade>, RemoveModifierCommand> removeModifierCommandFactory)
        {
            RemoveCatalogueItemCommandFactory = removeCatalogueItemCommandFactory;
            RemoveConditionItemCommandFactory = removeConditionItemCommandFactory;
            RemoveModifierCommandFactory = removeModifierCommandFactory;
        }

        private Func<IBindableMap<CatalogueItemFacade>, RemoveCatalogueItemCommand> RemoveCatalogueItemCommandFactory {
            get; }

        private Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> RemoveConditionItemCommandFactory {
            get; }

        private Func<IBindableMap<ModifierFacade>, RemoveModifierCommand> RemoveModifierCommandFactory { get; }

        #region Conditions

        public BindableMap<ConditionItemFacade, ICatalogueCondition> Create(
            INodeSimple<ICatalogueCondition> collection, string key = null)
        {
            return collection.ToBindableMap(ConditionItemFacadeExtensions.ToFacade, key,
                RemoveConditionItemCommandFactory);
        }

        public BindableMap<ConditionItemFacade, ICatalogueConditionGroup> Create(
            INodeSimple<ICatalogueConditionGroup> collection, string key = null)
        {
            return collection.ToBindableMap(ConditionItemFacadeExtensions.ToFacade, key,
                RemoveConditionItemCommandFactory);
        }

        #endregion

        #region Links

        public BindableMap<CatalogueItemFacade, IEntryLink> Create(IList<IEntryLink> collection,
            string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IGroupLink> Create(IList<IGroupLink> collection,
            string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IProfileLink> Create(IList<IProfileLink> collection,
            string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IRuleLink> Create(IList<IRuleLink> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IRootEntry> Create(IList<IRootEntry> collection,
            string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IRootLink> Create(IList<IRootLink> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key,
                RemoveCatalogueItemCommandFactory);
        }

        #endregion

        #region Modifiers

        public BindableMap<ModifierFacade, IEntryModifier> Create(IList<IEntryModifier> collection,
            string key = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, RemoveModifierCommandFactory);
        }

        public BindableMap<ModifierFacade, IGroupModifier> Create(IList<IGroupModifier> collection,
            string key = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, RemoveModifierCommandFactory);
        }

        public BindableMap<ModifierFacade, IProfileModifier> Create(IList<IProfileModifier> collection,
            string key = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, RemoveModifierCommandFactory);
        }

        public BindableMap<ModifierFacade, IRuleModifier> Create(IList<IRuleModifier> collection,
            string key = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, RemoveModifierCommandFactory);
        }

        #endregion

        #region Shared

        public BindableMap<CatalogueItemFacade, IEntry> CreateShared(IList<IEntry> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IGroup> CreateShared(IList<IGroup> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IProfile> CreateShared(IList<IProfile> collection,
            string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IRule> CreateShared(IList<IRule> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        #endregion

        #region Not Shared

        public BindableMap<CatalogueItemFacade, IEntry> Create(IList<IEntry> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IGroup> Create(IList<IGroup> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IProfile> Create(IList<IProfile> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        public BindableMap<CatalogueItemFacade, IRule> Create(IList<IRule> collection, string key = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key,
                RemoveCatalogueItemCommandFactory);
        }

        #endregion
    }
}
