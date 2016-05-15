// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using AppServices;
    using Bindables;
    using Demo;
    using Model;
    using Models;

    public class EntryViewModel : GenericViewModel<EntryViewModel, IEntry>, IRootItemViewModel,
        IModifiersListViewModel, ICatalogueItemsListViewModel
    {
        public EntryViewModel(ICommandsAggregateService commands, IEntry model = null)
            : base(model ?? ModelLocator.Entry)
        {
            Commands = commands;
            Entries = Entry.Entries.ToBindableMap("entries",
                Commands.RemoveCatalogueItemCommand.For(() => Entries));
            EntryLinks = Entry.EntryLinks.ToBindableMap("entry links",
                Commands.RemoveCatalogueItemCommand.For(() => EntryLinks));
            Groups = Entry.Groups.ToBindableMap("groups",
                Commands.RemoveCatalogueItemCommand.For(() => Groups));
            GroupLinks = Entry.GroupLinks.ToBindableMap("group links",
                Commands.RemoveCatalogueItemCommand.For(() => GroupLinks));
            Profiles = Entry.Profiles.ToBindableMap("profiles",
                Commands.RemoveCatalogueItemCommand.For(() => Profiles));
            ProfileLinks = Entry.ProfileLinks.ToBindableMap("profile links",
                Commands.RemoveCatalogueItemCommand.For(() => ProfileLinks));
            Rules = Entry.Rules.ToBindableMap("rules",
                Commands.RemoveCatalogueItemCommand.For(() => Rules));
            RuleLinks = Entry.RuleLinks.ToBindableMap("rule links",
                Commands.RemoveCatalogueItemCommand.For(() => RuleLinks));
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Entry.Id);
            Book = ViewModelLocator.BookIndexViewModel.WithModel(Entry.Book);
            Limits = ViewModelLocator.EntryLimitsViewModel.WithModel(Entry.Limits);
            RootEntry = Entry as IRootEntry;
            IsRootItem = RootEntry != null;
            Categories = Entry.Context.Catalogue.SystemContext.Categories.PrependWith(new NoCategory());
            Modifiers = Entry.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
            CreateCatalogueItemCommand = Commands.CreateCatalogueItemCommand.EnableFor(Entry);
        }

        public BookIndexViewModel Book { get; }

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

        private ICommandsAggregateService Commands { get; }

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

        public ICommand CreateCatalogueItemCommand { get; }

        public ICommand OpenCatalogueItemCommand => Commands.OpenCatalogueItemCommand;

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Entry.Modifiers);

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

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

        protected override EntryViewModel WithModelCore(IEntry model)
        {
            return new EntryViewModel(Commands, model);
        }
    }
}
