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

    public class EntryLinkViewModel : GenericViewModel<EntryLinkViewModel, IEntryLink>, IRootItemViewModel,
        IModifiersListViewModel
    {
        public EntryLinkViewModel(ICommandsAggregateService commands, IEntryLink model = null)
            : base(model ?? ModelLocator.EntryLink)
        {
            Commands = commands;
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Link.Id);
            RootLink = Link as IRootLink;
            IsRootItem = RootLink != null;
            Categories = Link.Context.Catalogue.SystemContext.Categories.PrependWith(new NoCategory());
            Modifiers = Link.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
        }

        public IdentifierViewModel Id { get; }


        public BindableMap<ModifierFacade, IEntryModifier> Modifiers { get; }

        public ICommand OpenLinkTargetAsChildCommand
            => Commands.OpenLinkTargetAsChildCommand.SetParameter(Link.ToFacade());

        public ICommand OpenLinkTargetAsSharedCommand
            => Commands.OpenLinkTargetAsSharedCommand.SetParameter(Link.ToFacade());

        public IEnumerable<IEntry> SharedEntries => Link.Context.Catalogue.SharedEntries;

        public IEntry TargetEntry
        {
            get { return Link.Target; }
            set { Set(() => Link.Target == value, () => Link.Target = value); }
        }

        private ICommandsAggregateService Commands { get; }

        private IEntryLink Link => Model;

        private IRootLink RootLink { get; }

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Link.Modifiers);

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        public IEnumerable<ICategory> Categories { get; }

        public ICategory Category
        {
            get { return RootLink?.CategoryLink.Target; }
            set
            {
                if (!IsRootItem)
                {
                    return;
                }
                Set(() => RootLink.CategoryLink.Target == value, () => RootLink.CategoryLink.Target = value);
            }
        }

        public bool IsRootItem { get; }

        protected override EntryLinkViewModel WithModelCore(IEntryLink model)
        {
            return new EntryLinkViewModel(Commands, model);
        }
    }
}
