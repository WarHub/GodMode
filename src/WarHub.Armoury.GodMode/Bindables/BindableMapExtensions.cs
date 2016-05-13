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
        public static BindableMap<TFacade, TModel> ToBindableMap<TFacade, TModel>(this IList<TModel> models,
            Func<TModel, TFacade> mapFunc, Func<TFacade, TModel> reverseMap, string key = null,
            ICommand<IModelFacade> removeCommand = null)
        {
            return new BindableMap<TFacade, TModel>(models, mapFunc, reverseMap, key);
        }

        public static BindableMap<TFacade, TModel> ToBindableMap<TFacade, TModel>(this IList<TModel> models,
            Func<TModel, TFacade> mapFunc, string key = null, ICommand<TFacade> removeCommand = null)
            where TFacade : IModelFacade
        {
            return new BindableMap<TFacade, TModel>(models, x =>
            {
                var facade = mapFunc(x);
                facade.RemoveCommand = removeCommand?.SetParameter(facade) ?? new RelayCommand(() => { }, () => false);
                return facade;
            }, facade => (TModel) facade.Model, key);
        }

        public static BindableMap<ConditionItemFacade, ICatalogueCondition> ToBindableMap(
            this INodeSimple<ICatalogueCondition> collection, string key = null,
            ICommand<ConditionItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(condition => condition.ToFacade(), key, removeCommand);
        }

        public static BindableMap<ConditionItemFacade, ICatalogueConditionGroup> ToBindableMap(
            this INodeSimple<ICatalogueConditionGroup> collection, string key = null,
            ICommand<ConditionItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(group => group.ToFacade(), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IEntry> ToBindableMap(this IList<IEntry> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null, bool asShared = false)
        {
            return collection.ToBindableMap(entry => entry.ToFacade(asShared), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IGroup> ToBindableMap(this IList<IGroup> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null, bool asShared = false)
        {
            return collection.ToBindableMap(group => group.ToFacade(asShared), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IProfile> ToBindableMap(this IList<IProfile> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null, bool asShared = false)
        {
            return collection.ToBindableMap(profile => profile.ToFacade(asShared), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRule> ToBindableMap(this IList<IRule> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null, bool asShared = false)
        {
            return collection.ToBindableMap(rule => rule.ToFacade(asShared), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IEntryLink> ToBindableMap(this IList<IEntryLink> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(link => link.ToFacade(), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IGroupLink> ToBindableMap(this IList<IGroupLink> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(link => link.ToFacade(), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IProfileLink> ToBindableMap(this IList<IProfileLink> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(link => link.ToFacade(), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRuleLink> ToBindableMap(this IList<IRuleLink> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(link => link.ToFacade(), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRootEntry> ToBindableMap(this IList<IRootEntry> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(entry => entry.ToFacade(), key, removeCommand);
        }

        public static BindableMap<CatalogueItemFacade, IRootLink> ToBindableMap(this IList<IRootLink> collection,
            string key = null, ICommand<CatalogueItemFacade> removeCommand = null)
        {
            return collection.ToBindableMap(link => link.ToFacade(), key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IEntryModifier> ToBindableMap(this IList<IEntryModifier> collection,
            string key = null, ICommand<ModifierFacade> removeCommand = null)
        {
            return collection.ToBindableMap(modifier => modifier.ToFacade(), key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IGroupModifier> ToBindableMap(this IList<IGroupModifier> collection,
            string key = null, ICommand<ModifierFacade> removeCommand = null)
        {
            return collection.ToBindableMap(modifier => modifier.ToFacade(), key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IProfileModifier> ToBindableMap(
            this IList<IProfileModifier> collection, string key = null, ICommand<ModifierFacade> removeCommand = null)
        {
            return collection.ToBindableMap(modifier => modifier.ToFacade(), key, removeCommand);
        }

        public static BindableMap<ModifierFacade, IRuleModifier> ToBindableMap(this IList<IRuleModifier> collection,
            string key = null, ICommand<ModifierFacade> removeCommand = null)
        {
            return collection.ToBindableMap(modifier => modifier.ToFacade(), key, removeCommand);
        }
    }
}
