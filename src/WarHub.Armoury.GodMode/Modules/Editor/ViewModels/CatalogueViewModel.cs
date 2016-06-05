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
    using Model.Repo;
    using Models;

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class CatalogueViewModel : GenericViewModel<ICatalogue>, ICatalogueItemsListViewModel
    {
        public CatalogueViewModel(ICatalogue model, Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            Func<IBindableMap<CatalogueItemFacade>, RemoveCatalogueItemCommand> removeCommandFactory,
            CreateItemInCatalogueCommandFactory createItemInCatalogueCommandFactory,
            OpenCatalogueItemCommand openCatalogueItemCommand, SaveCatalogueCommand saveCatalogueCommand)
            : base(model)
        {
            OpenCatalogueItemCommand = openCatalogueItemCommand;
            SaveCatalogueCommand = saveCatalogueCommand;
            SharedEntries = Catalogue.SharedEntries.ToBindableMap("shared entries", removeCommandFactory);
            SharedGroups = Catalogue.SharedGroups.ToBindableMap("shared groups", removeCommandFactory);
            SharedProfiles = Catalogue.SharedProfiles.ToBindableMap("shared profiles", removeCommandFactory);
            SharedRules = Catalogue.SharedRules.ToBindableMap("shared rules", removeCommandFactory);
            Entries = Catalogue.Entries.ToBindableMap("entries", removeCommandFactory);
            EntryLinks = Catalogue.EntryLinks.ToBindableMap("entry links", removeCommandFactory);
            Rules = Catalogue.Rules.ToBindableMap("rules", removeCommandFactory);
            RuleLinks = Catalogue.RuleLinks.ToBindableMap("rule links", removeCommandFactory);
            Id = identifierVmFactory(Catalogue.Id);
            CreateCatalogueItemCommand = createItemInCatalogueCommandFactory(Catalogue);
            CatalogueInfo = new CatalogueInfo(Catalogue);
        }

        public CatalogueInfo CatalogueInfo { get; }

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

        public SaveCatalogueCommand SaveCatalogueCommand { get; }

        private ICatalogue Catalogue => Model;

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

        public CreateCatalogueItemCommandBase CreateCatalogueItemCommand { get; }

        public OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }
    }
}
