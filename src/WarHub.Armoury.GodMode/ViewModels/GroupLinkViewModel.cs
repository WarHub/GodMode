// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Demo;
    using Model;
    using ModelFacades;
    using Models;
    using Services;

    public class GroupLinkViewModel : GenericViewModel<GroupLinkViewModel, IGroupLink>, IModifiersListViewModel
    {
        public GroupLinkViewModel(ICommandsAggregateService commands, IGroupLink model = null)
            : base(model ?? ModelLocator.GroupLink)
        {
            Commands = commands;
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Link.Id);
            Modifiers = Link.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
        }

        public IdentifierViewModel Id { get; }

        public BindableMap<ModifierFacade, IGroupModifier> Modifiers { get; }

        public ICommand OpenLinkTargetAsChildCommand
            => Commands.OpenLinkTargetAsChildCommand.SetParameter(Link.ToFacade());

        public ICommand OpenLinkTargetAsSharedCommand
            => Commands.OpenLinkTargetAsSharedCommand.SetParameter(Link.ToFacade());

        public IEnumerable<IGroup> SharedGroups => Link.Context.Catalogue.SharedGroups;

        public IGroup TargetGroup
        {
            get { return Link.Target; }
            set { Set(() => Link.Target == value, () => Link.Target = value); }
        }

        private ICommandsAggregateService Commands { get; }

        private IGroupLink Link => Model;

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Link.Modifiers);

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        protected override GroupLinkViewModel WithModelCore(IGroupLink model)
        {
            return new GroupLinkViewModel(Commands, model);
        }
    }
}
