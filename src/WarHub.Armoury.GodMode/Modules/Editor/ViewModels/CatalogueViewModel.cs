// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Bindables;
    using Demo;
    using Model;
    using Models;
    using Services;

    public class CatalogueViewModel : GenericViewModel<CatalogueViewModel, ICatalogue>, ICatalogueItemsListViewModel
    {
        public CatalogueViewModel(ICommandsAggregateService commands, ICatalogue catalogue = null)
            : base(catalogue ?? ModelLocator.Catalogue)
        {
            Commands = commands;
            SharedEntries = Catalogue.SharedEntries.ToBindableMap("shared entries", asShared: true,
                removeCommand: Commands.RemoveCatalogueItemCommand.For(() => SharedEntries));
            SharedGroups = Catalogue.SharedGroups.ToBindableMap("shared groups", asShared: true,
                removeCommand: Commands.RemoveCatalogueItemCommand.For(() => SharedGroups));
            SharedProfiles = Catalogue.SharedProfiles.ToBindableMap("shared profiles", asShared: true,
                removeCommand: Commands.RemoveCatalogueItemCommand.For(() => SharedProfiles));
            SharedRules = Catalogue.SharedRules.ToBindableMap("shared rules", asShared: true,
                removeCommand: Commands.RemoveCatalogueItemCommand.For(() => SharedRules));
            Entries = Catalogue.Entries.ToBindableMap("entries",
                Commands.RemoveCatalogueItemCommand.For(() => Entries));
            EntryLinks = Catalogue.EntryLinks.ToBindableMap("entry links",
                Commands.RemoveCatalogueItemCommand.For(() => EntryLinks));
            Rules = Catalogue.Rules.ToBindableMap("rules",
                Commands.RemoveCatalogueItemCommand.For(() => Rules));
            RuleLinks = Catalogue.RuleLinks.ToBindableMap("rule links",
                Commands.RemoveCatalogueItemCommand.For(() => RuleLinks));
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Catalogue.Id);
            CreateCatalogueItemCommand = Commands.CreateCatalogueItemCommand.EnableFor(Catalogue);
        }

        public string AuthorContact
        {
            get { return Catalogue.Author.Contact; }
            set { Set(() => Catalogue.Author.Contact == value, () => Catalogue.Author.Contact = value); }
        }

        public string AuthorName
        {
            get { return Catalogue.Author.Name; }
            set { Set(() => Catalogue.Author.Name == value, () => Catalogue.Author.Name = value); }
        }

        public string AuthorWebsite
        {
            get { return Catalogue.Author.Website; }
            set { Set(() => Catalogue.Author.Website == value, () => Catalogue.Author.Website = value); }
        }

        public string Books
        {
            get { return Catalogue.BookSources; }
            set { Set(() => Catalogue.BookSources == value, () => Catalogue.BookSources = value); }
        }

        public IdentifierViewModel Id { get; }

        public string Name
        {
            get { return Catalogue.Name; }
            set { Set(() => Catalogue.Name == value, () => Catalogue.Name = value); }
        }

        public int Revision
        {
            get { return (int) Catalogue.Revision; }
            set
            {
                var uintValue = (uint) value;
                Set(() => Catalogue.Revision == uintValue, () => Catalogue.Revision = uintValue);
            }
        }

        private ICatalogue Catalogue => Model;

        private ICommandsAggregateService Commands { get; }

        private BindableMap<CatalogueItemFacade, IRootEntry> Entries { get; }

        private BindableMap<CatalogueItemFacade, IRootLink> EntryLinks { get; }

        private BindableMap<CatalogueItemFacade, IRuleLink> RuleLinks { get; }

        private BindableMap<CatalogueItemFacade, IRule> Rules { get; }

        private BindableMap<CatalogueItemFacade, IEntry> SharedEntries { get; }

        private BindableMap<CatalogueItemFacade, IGroup> SharedGroups { get; }

        private BindableMap<CatalogueItemFacade, IProfile> SharedProfiles { get; }

        private BindableMap<CatalogueItemFacade, IRule> SharedRules { get; }

        public IEnumerable<IBindableGrouping<CatalogueItemFacade>> CatalogueItems
        {
            get
            {
                yield return SharedEntries;
                yield return SharedGroups;
                yield return SharedProfiles;
                yield return SharedRules;
                yield return Entries;
                yield return EntryLinks;
                yield return Rules;
                yield return RuleLinks;
            }
        }

        public ICommand CreateCatalogueItemCommand { get; }

        public ICommand OpenCatalogueItemCommand => Commands.OpenCatalogueItemCommand;


        protected override CatalogueViewModel WithModelCore(ICatalogue model)
        {
            return new CatalogueViewModel(Commands, model);
        }
    }
}
