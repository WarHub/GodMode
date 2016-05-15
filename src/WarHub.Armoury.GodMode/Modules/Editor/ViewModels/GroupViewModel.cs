// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using AppServices;
    using Bindables;
    using Demo;
    using Model;
    using Models;

    public class GroupViewModel : GenericViewModel<GroupViewModel, IGroup>, IModifiersListViewModel,
        ICatalogueItemsListViewModel
    {
        private CatalogueItemFacade _defaultChoice;
        private IEnumerable<CatalogueItemFacade> _defaultChoices;

        public GroupViewModel(ICommandsAggregateService commands, IGroup model = null)
            : base(model ?? ModelLocator.Group)
        {
            Commands = commands;
            Entries = Group.Entries.ToBindableMap("entries",
                Commands.RemoveCatalogueItemCommand.For(() => Entries));
            EntryLinks = Group.EntryLinks.ToBindableMap("entry links",
                Commands.RemoveCatalogueItemCommand.For(() => EntryLinks));
            Groups = Group.Groups.ToBindableMap("groups",
                Commands.RemoveCatalogueItemCommand.For(() => Groups));
            GroupLinks = Group.GroupLinks.ToBindableMap("group links",
                Commands.RemoveCatalogueItemCommand.For(() => GroupLinks));
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Group.Id);
            Limits = ViewModelLocator.EntryLimitsViewModel.WithModel(Group.Limits);
            Modifiers = Group.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
            DefaultChoices =
                Group.GetEntryLinkPairs()
                    .Select(pair => pair.HasLink ? pair.Link.ToFacade() : pair.Entry.ToFacade())
                    .PrependWith(NoChoiceItemFacade)
                    .ToArray();
            DefaultChoice = Group.DefaultChoice == null
                ? NoChoiceItemFacade
                : DefaultChoices.First(
                    facade =>
                        Group.DefaultChoice.Equals(facade.IsLink
                            ? ((IEntryLink) facade.Item).Target
                            : (IEntry) facade.Item));
            CreateCatalogueItemCommand = Commands.CreateCatalogueItemCommand.EnableFor(Group);
        }

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

        private ICommandsAggregateService Commands { get; }

        private BindableMap<CatalogueItemFacade, IEntry> Entries { get; }

        private BindableMap<CatalogueItemFacade, IEntryLink> EntryLinks { get; }

        private IGroup Group => Model;

        private BindableMap<CatalogueItemFacade, IGroupLink> GroupLinks { get; }

        private BindableMap<CatalogueItemFacade, IGroup> Groups { get; }

        private CatalogueItemFacade NoChoiceItemFacade { get; } = new CatalogueItemFacade(null, CatalogueItemKind.Entry,
            () => "None");

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

        public ICommand CreateCatalogueItemCommand { get; }

        public ICommand OpenCatalogueItemCommand => Commands.OpenCatalogueItemCommand;

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Group.Modifiers);

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        private void OnDefaultChoiceChanged()
        {
            Group.DefaultChoice = DefaultChoice.IsLink
                ? ((IEntryLink) DefaultChoice.Item).Target
                : (IEntry) DefaultChoice.Item;
        }


        protected override GroupViewModel WithModelCore(IGroup model)
        {
            return new GroupViewModel(Commands, model);
        }
    }
}
