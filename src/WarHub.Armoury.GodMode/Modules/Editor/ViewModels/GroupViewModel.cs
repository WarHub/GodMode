// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Bindables;
    using Commands;
    using Model;
    using Models;
    using Mvvm.Commands;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class GroupViewModel : GenericViewModel<IGroup>, IModifiersListViewModel, ICatalogueItemsListViewModel
    {
        private CatalogueItemFacade _defaultChoice;
        private IEnumerable<CatalogueItemFacade> _defaultChoices;

        public GroupViewModel(IGroup model,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            Func<IEntryLimits, EntryLimitsViewModel> entryLimitsVmFactory,
            OpenCatalogueItemCommand openCatalogueItemCommand,
            OpenModifierCommand openModifierCommand,
            BindableMapBuilder bindableMapBuilder,
            CreateItemInGroupCommandFactory createItemInGroupCommandFactory,
            CreateGroupModifierCommandFactory createGroupModifierCommandFactory)
            : base(model)
        {
            OpenCatalogueItemCommand = openCatalogueItemCommand;
            OpenModifierCommand = openModifierCommand;
            Entries = bindableMapBuilder.Create(Group.Entries, "entries");
            EntryLinks = bindableMapBuilder.Create(Group.EntryLinks, "entry links");
            Groups = bindableMapBuilder.Create(Group.Groups, "groups");
            GroupLinks = bindableMapBuilder.Create(Group.GroupLinks, "group links");
            Id = identifierVmFactory(Group.Id);
            Limits = entryLimitsVmFactory(Group.Limits);
            Modifiers = bindableMapBuilder.Create(Group.Modifiers);
            DefaultChoices = CreateDefaultChoices();
            DefaultChoice = GetDefaultChoice();
            CreateCatalogueItemCommand = createItemInGroupCommandFactory(Group);
            CreateModifierCommand = createGroupModifierCommandFactory(Group.Modifiers);
        }

        public CreateGroupModifierCommand CreateModifierCommand { get; }

        public CatalogueItemFacade DefaultChoice
        {
            get { return _defaultChoice; }
            set
            {
                if (Set(ref _defaultChoice, value))
                {
                    OnDefaultChoiceChanged();
                }
            }
        }

        public IEnumerable<CatalogueItemFacade> DefaultChoices
        {
            get { return _defaultChoices; }
            private set { Set(ref _defaultChoices, value); }
        }

        public IdentifierViewModel Id { get; }

        public bool IsCollective
        {
            get { return Group.IsCollective; }
            set { Set(() => Group.IsCollective == value, () => Group.IsCollective = value); }
        }

        public bool IsHidden
        {
            get { return Group.IsHidden; }
            set { Set(() => Group.IsHidden == value, () => Group.IsHidden = value); }
        }

        public EntryLimitsViewModel Limits { get; }

        public BindableMap<ModifierFacade, IGroupModifier> Modifiers { get; }

        public string Name
        {
            get { return Group.Name; }
            set { Set(() => Group.Name == value, () => Group.Name = value); }
        }

        private BindableMap<CatalogueItemFacade, IEntry> Entries { get; }

        private BindableMap<CatalogueItemFacade, IEntryLink> EntryLinks { get; }

        private IGroup Group => Model;

        private BindableMap<CatalogueItemFacade, IGroupLink> GroupLinks { get; }

        private BindableMap<CatalogueItemFacade, IGroup> Groups { get; }

        private CatalogueItemFacade NoChoiceItemFacade { get; } = new CatalogueItemFacade(null, CatalogueItemKind.Entry,
            null, () => "None");

        public IEnumerable<IBindableGrouping<CatalogueItemFacade>> CatalogueItems
        {
            get
            {
                yield return Entries;
                yield return EntryLinks;
                yield return Groups;
                yield return GroupLinks;
            }
        }

        public CreateCatalogueItemCommandBase CreateCatalogueItemCommand { get; }

        public OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }

        ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        private IEnumerable<CatalogueItemFacade> CreateDefaultChoices()
        {
            return Group.GetEntryLinkPairs()
                .Select(pair => pair.HasLink ? pair.Link.ToFacade(null) : pair.Entry.ToFacade(null))
                .PrependWith(NoChoiceItemFacade)
                .ToArray();
        }

        private CatalogueItemFacade GetDefaultChoice()
        {
            return Group.DefaultChoice == null
                ? NoChoiceItemFacade
                : DefaultChoices.First(
                    facade =>
                        Group.DefaultChoice.Equals(facade.IsLink
                            ? ((IEntryLink) facade.Item).Target
                            : (IEntry) facade.Item));
        }

        private void OnDefaultChoiceChanged()
        {
            Group.DefaultChoice = DefaultChoice.IsLink
                ? ((IEntryLink) DefaultChoice.Item).Target
                : (IEntry) DefaultChoice.Item;
        }
    }
}
