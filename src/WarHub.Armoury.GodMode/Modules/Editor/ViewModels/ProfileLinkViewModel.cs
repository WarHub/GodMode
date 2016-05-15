// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using AppServices;
    using Bindables;
    using Model;
    using Models;

    public class ProfileLinkViewModel : GenericViewModel<IProfileLink>, IModifiersListViewModel
    {
        public ProfileLinkViewModel(IProfileLink model, ICommandsAggregateService commands,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory) : base(model)
        {
            Commands = commands;
            Id = identifierVmFactory(Link.Id);
            Modifiers = Link.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
        }

        public IdentifierViewModel Id { get; }

        public BindableMap<ModifierFacade, IProfileModifier> Modifiers { get; }

        public ICommand OpenLinkTargetAsChildCommand
            => Commands.OpenLinkTargetAsChildCommand.SetParameter(Link.ToFacade());

        public ICommand OpenLinkTargetAsSharedCommand
            => Commands.OpenLinkTargetAsSharedCommand.SetParameter(Link.ToFacade());

        public IProfile Target
        {
            get { return Link.Target; }
            set { Set(() => Link.Target == value, () => Link.Target = value); }
        }

        public IEnumerable<IProfile> Targets => Link.Context.Catalogue.SharedProfiles;

        private ICommandsAggregateService Commands { get; }

        private IProfileLink Link => Model;

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Link.Modifiers);

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;
    }
}
