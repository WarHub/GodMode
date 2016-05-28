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

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class EntryLinkViewModel : GenericViewModel<IEntryLink>, IRootItemViewModel, IModifiersListViewModel
    {
        public EntryLinkViewModel(IEntryLink model,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            CreateEntryModifierCommandFactory createModifierCommandFactory,
            OpenModifierCommand openModifierCommand,
            Func<IBindableMap<ModifierFacade>, RemoveModifierCommand> removeModifierCommandFactory,
            OpenLinkTargetAsChildCommand openLinkTargetAsChildCommand,
            OpenLinkTargetAsSharedCommand openLinkTargetAsSharedCommand)
            : base(model)
        {
            OpenModifierCommand = openModifierCommand;
            CreateModifierCommand = createModifierCommandFactory(Link.Modifiers);
            Id = identifierVmFactory(Link.Id);
            RootLink = Link as IRootLink;
            IsRootItem = RootLink != null;
            Categories = Link.Context.Catalogue.SystemContext.Categories.PrependWith(new NoCategory());
            Modifiers = Link.Modifiers.ToBindableMap(null, removeModifierCommandFactory);
            OpenLinkTargetAsChildCommand = openLinkTargetAsChildCommand.SetParameter(Link.ToFacade(null));
            OpenLinkTargetAsSharedCommand = openLinkTargetAsSharedCommand.SetParameter(Link.ToFacade(null));
        }

        public CreateEntryModifierCommand CreateModifierCommand { get; }

        public IdentifierViewModel Id { get; }

        public BindableMap<ModifierFacade, IEntryModifier> Modifiers { get; }

        public ICommand OpenLinkTargetAsChildCommand { get; }

        public ICommand OpenLinkTargetAsSharedCommand { get; }

        public IEnumerable<IEntry> SharedEntries => Link.Context.Catalogue.SharedEntries;

        public IEntry TargetEntry
        {
            get { return Link.Target; }
            set { Set(() => Link.Target == value, () => Link.Target = value); }
        }

        private IEntryLink Link => Model;

        private IRootLink RootLink { get; }

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

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
    }
}
