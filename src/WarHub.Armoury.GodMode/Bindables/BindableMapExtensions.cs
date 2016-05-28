// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Bindables
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Modules.Editor.Models;
    using Mvvm.Commands;

    public static class BindableMapExtensions
    {
        private static ICommand<TFacade> GetDisabledCommand<TFacade>()
        {
            return new RelayCommand<TFacade>(_ => { }, _ => false);
        }

        public static BindableMap<TFacade, TModel> ToBindableMap<TFacade, TModel>(
            this IList<TModel> models, FacadeFactory<TModel, TFacade> facadeFactory, string key = null,
            Func<IBindableMap<TFacade>, ICommand<TFacade>> removeCommandFactory = null) where TFacade : IModelFacade
        {
            removeCommandFactory = removeCommandFactory ?? (_ => GetDisabledCommand<TFacade>());
            return new BindableMap<TFacade, TModel>(removeCommandFactory, models, facadeFactory,
                facade => (TModel) facade.Model, key);
        }

        #region Conditions

        public static BindableMap<ConditionItemFacade, ICatalogueCondition> ToBindableMap(
            this INodeSimple<ICatalogueCondition> collection, string key = null,
            Func<IBindableMap<ConditionItemFacade>, ICommand<ConditionItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(ConditionItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<ConditionItemFacade, ICatalogueConditionGroup> ToBindableMap(
            this INodeSimple<ICatalogueConditionGroup> collection, string key = null,
            Func<IBindableMap<ConditionItemFacade>, ICommand<ConditionItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(ConditionItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        #endregion

        #region Links

        public static BindableMap<CatalogueItemFacade, IEntryLink> ToBindableMap(
            this IList<IEntryLink> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IGroupLink> ToBindableMap(
            this IList<IGroupLink> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IProfileLink> ToBindableMap(
            this IList<IProfileLink> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRuleLink> ToBindableMap(
            this IList<IRuleLink> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRootEntry> ToBindableMap(
            this IList<IRootEntry> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRootLink> ToBindableMap(
            this IList<IRootLink> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacade, key, removeCommand);
        }

        #endregion

        #region Modifiers

        public static BindableMap<ModifierFacade, IEntryModifier> ToBindableMap(
            this IList<IEntryModifier> collection, string key = null,
            Func<IBindableMap<ModifierFacade>, ICommand<ModifierFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IGroupModifier> ToBindableMap(
            this IList<IGroupModifier> collection, string key = null,
            Func<IBindableMap<ModifierFacade>, ICommand<ModifierFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IProfileModifier> ToBindableMap(
            this IList<IProfileModifier> collection, string key = null,
            Func<IBindableMap<ModifierFacade>, ICommand<ModifierFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IRuleModifier> ToBindableMap(
            this IList<IRuleModifier> collection, string key = null,
            Func<IBindableMap<ModifierFacade>, ICommand<ModifierFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(ModifierFacadeExtensions.ToFacade, key, removeCommand);
        }

        #endregion

        #region Shared

        public static BindableMap<CatalogueItemFacade, IEntry> ToBindableMapShared(
            this IList<IEntry> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IGroup> ToBindableMapShared(
            this IList<IGroup> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IProfile> ToBindableMapShared(
            this IList<IProfile> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRule> ToBindableMapShared(
            this IList<IRule> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        #endregion

        #region Not Shared

        public static BindableMap<CatalogueItemFacade, IEntry> ToBindableMap(
            this IList<IEntry> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IGroup> ToBindableMap(
            this IList<IGroup> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IProfile> ToBindableMap(
            this IList<IProfile> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRule> ToBindableMap(
            this IList<IRule> collection,
            string key = null,
            Func<IBindableMap<CatalogueItemFacade>, ICommand<CatalogueItemFacade>> removeCommand = null)
        {
            return collection.ToBindableMap(CatalogueItemFacadeExtensions.ToFacadeShared, key, removeCommand);
        }

        #endregion
    }
}
