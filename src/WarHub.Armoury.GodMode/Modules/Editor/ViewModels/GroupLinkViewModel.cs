// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Bindables;
    using Commands;
    using Model;
    using Models;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class GroupLinkViewModel : GenericViewModel<IGroupLink>, IModifiersListViewModel
    {
        public GroupLinkViewModel(IGroupLink model, Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            Func<IBindableMap<ModifierFacade>, RemoveModifierCommand> removeCommandFactory,
            CreateGroupModifierCommandFactory createGroupModifierCommandFactory,
            OpenModifierCommand openModifierCommand, OpenLinkTargetAsChildCommand openLinkTargetAsChildCommand,
            OpenLinkTargetAsSharedCommand openLinkTargetAsSharedCommand) : base(model)
        {
            OpenModifierCommand = openModifierCommand;
            Id = identifierVmFactory(Link.Id);
            Modifiers = Link.Modifiers.ToBindableMap(removeCommand: removeCommandFactory);
            CreateModifierCommand = createGroupModifierCommandFactory(Link.Modifiers);
            OpenLinkTargetAsChildCommand = openLinkTargetAsChildCommand.SetParameter(Link.ToFacade(null));
            OpenLinkTargetAsSharedCommand = openLinkTargetAsSharedCommand.SetParameter(Link.ToFacade(null));
        }

        public CreateGroupModifierCommand CreateModifierCommand { get; }

        public IdentifierViewModel Id { get; }

        public BindableMap<ModifierFacade, IGroupModifier> Modifiers { get; }

        public ICommand OpenLinkTargetAsChildCommand { get; }

        public ICommand OpenLinkTargetAsSharedCommand { get; }

        public IEnumerable<IGroup> SharedGroups => Link.Context.Catalogue.SharedGroups;

        public IGroup TargetGroup
        {
            get { return Link.Target; }
            set { Set(() => Link.Target == value, () => Link.Target = value); }
        }

        private IGroupLink Link => Model;

        Mvvm.Commands.ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;
    }
}
