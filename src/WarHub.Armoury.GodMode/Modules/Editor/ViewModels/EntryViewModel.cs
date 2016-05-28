// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Bindables;
    using Commands;
    using Model;
    using Models;
    using Mvvm.Commands;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class EntryViewModel : GenericViewModel<IEntry>, IRootItemViewModel,
        IModifiersListViewModel, ICatalogueItemsListViewModel
    {
        public EntryViewModel(IEntry model,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            Func<IBookIndex, BookIndexViewModel> bookIndexVmFactory,
            Func<IEntryLimits, EntryLimitsViewModel> entryLimitsVmFactory,
            CreateItemInEntryCommandFactory createItemInEntryCommandFactory,
            BindableMapBuilder bindableMapBuilder,
            OpenCatalogueItemCommand openCatalogueItemCommand, OpenModifierCommand openModifierCommand,
            CreateEntryModifierCommandFactory createEntryModifierCommandFactory)
            : base(model)
        {
            Entries = bindableMapBuilder.Create(Entry.Entries, "entries");
            EntryLinks = bindableMapBuilder.Create(Entry.EntryLinks, "entry links");
            Groups = bindableMapBuilder.Create(Entry.Groups, "groups");
            GroupLinks = bindableMapBuilder.Create(Entry.GroupLinks, "group links");
            Profiles = bindableMapBuilder.Create(Entry.Profiles, "profiles");
            ProfileLinks = bindableMapBuilder.Create(Entry.ProfileLinks, "profile links");
            Rules = bindableMapBuilder.Create(Entry.Rules, "rules");
            RuleLinks = bindableMapBuilder.Create(Entry.RuleLinks, "rule links");
            Id = identifierVmFactory(Entry.Id);
            Book = bookIndexVmFactory(Entry.Book);
            Limits = entryLimitsVmFactory(Entry.Limits);
            RootEntry = Entry as IRootEntry;
            IsRootItem = RootEntry != null;
            Categories = Entry.Context.Catalogue.SystemContext.Categories.PrependWith(new NoCategory());
            Modifiers = bindableMapBuilder.Create(Entry.Modifiers);

            OpenCatalogueItemCommand = openCatalogueItemCommand;
            OpenModifierCommand = openModifierCommand;
            CreateCatalogueItemCommand = createItemInEntryCommandFactory(Entry);
            CreateModifierCommand = createEntryModifierCommandFactory(Entry.Modifiers);
        }

        public BookIndexViewModel Book { get; }

        public CreateEntryModifierCommand CreateModifierCommand { get; }

        public EntryType EntryType
        {
            get { return Entry.Type; }
            set { Set(() => Entry.Type == value, () => Entry.Type = value); }
        }

        public IEnumerable<EntryType> EntryTypes
        {
            get
            {
                yield return EntryType.Model;
                yield return EntryType.Unit;
                yield return EntryType.Upgrade;
            }
        }

        public IdentifierViewModel Id { get; }

        public bool IsCollective
        {
            get { return Entry.IsCollective; }
            set { Set(() => Entry.IsCollective == value, () => Entry.IsCollective = value); }
        }

        public bool IsHidden
        {
            get { return Entry.IsHidden; }
            set { Set(() => Entry.IsHidden == value, () => Entry.IsHidden = value); }
        }

        public EntryLimitsViewModel Limits { get; }

        public BindableMap<ModifierFacade, IEntryModifier> Modifiers { get; }

        public string Name
        {
            get { return Entry.Name; }
            set { Set(() => Entry.Name == value, () => Entry.Name = value); }
        }

        public decimal PointCost
        {
            get { return Entry.PointCost; }
            set { Set(() => Entry.PointCost == value, () => Entry.PointCost = value); }
        }

        private BindableMap<CatalogueItemFacade, IEntry> Entries { get; }

        private IEntry Entry => Model;

        private BindableMap<CatalogueItemFacade, IEntryLink> EntryLinks { get; }

        private BindableMap<CatalogueItemFacade, IGroupLink> GroupLinks { get; }

        private BindableMap<CatalogueItemFacade, IGroup> Groups { get; }

        private BindableMap<CatalogueItemFacade, IProfileLink> ProfileLinks { get; }

        private BindableMap<CatalogueItemFacade, IProfile> Profiles { get; }

        private IRootEntry RootEntry { get; }

        private BindableMap<CatalogueItemFacade, IRuleLink> RuleLinks { get; }

        private BindableMap<CatalogueItemFacade, IRule> Rules { get; }

        public IEnumerable<IBindableGrouping<CatalogueItemFacade>> CatalogueItems
        {
            get
            {
                yield return Entries;
                yield return EntryLinks;
                yield return Groups;
                yield return GroupLinks;
                yield return Profiles;
                yield return ProfileLinks;
                yield return Rules;
                yield return RuleLinks;
            }
        }

        public CreateCatalogueItemCommandBase CreateCatalogueItemCommand { get; }

        public OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }

        ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        public IEnumerable<ICategory> Categories { get; }

        public ICategory Category
        {
            get { return RootEntry?.CategoryLink.Target; }
            set
            {
                if (!IsRootItem)
                {
                    return;
                }
                Set(() => RootEntry.CategoryLink.Target == value, () => RootEntry.CategoryLink.Target = value);
            }
        }

        public bool IsRootItem { get; }
    }
}
