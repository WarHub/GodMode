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

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RuleLinkViewModel : GenericViewModel<IRuleLink>, IModifiersListViewModel
    {
        public RuleLinkViewModel(IRuleLink model,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            BindableMapBuilder bindableMapBuilder,
            OpenModifierCommand openModifierCommand,
            CreateRuleModifierCommandFactory createRuleModifierCommandFactory,
            OpenLinkTargetAsChildCommand openLinkTargetAsChildCommand,
            OpenLinkTargetAsSharedCommand openLinkTargetAsSharedCommand)
            : base(model)
        {
            Id = identifierVmFactory(Link.Id);
            OpenModifierCommand = openModifierCommand;
            Modifiers = bindableMapBuilder.Create(Link.Modifiers);
            CreateModifierCommand = createRuleModifierCommandFactory(Link.Modifiers);
            OpenLinkTargetAsChildCommand = openLinkTargetAsChildCommand.SetParameter(Link.ToFacade(null));
            OpenLinkTargetAsSharedCommand = openLinkTargetAsSharedCommand.SetParameter(Link.ToFacade(null));
        }

        public CreateRuleModifierCommand CreateModifierCommand { get; }

        public IdentifierViewModel Id { get; }

        public BindableMap<ModifierFacade, IRuleModifier> Modifiers { get; }

        public ICommand OpenLinkTargetAsChildCommand { get; }

        public ICommand OpenLinkTargetAsSharedCommand { get; }

        public IRule Target
        {
            get { return Link.Target; }
            set { Set(() => Link.Target == value, () => Link.Target = value); }
        }

        public IEnumerable<IRule> Targets => Link.Context.Catalogue.SharedRules;

        private IRuleLink Link => Model;

        Mvvm.Commands.ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;
    }
}
