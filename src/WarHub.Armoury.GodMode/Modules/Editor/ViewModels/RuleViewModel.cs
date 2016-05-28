// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Bindables;
    using Commands;
    using Model;
    using Models;
    using Mvvm.Commands;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RuleViewModel : GenericViewModel<IRule>, IModifiersListViewModel
    {
        public RuleViewModel(IRule model,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            Func<IBookIndex, BookIndexViewModel> bookIndexVmFactory,
            BindableMapBuilder bindableMapBuilder,
            OpenModifierCommand openModifierCommand,
            CreateRuleModifierCommandFactory createRuleModifierCommandFactory) : base(model)
        {
            Id = identifierVmFactory(Rule.Id);
            Book = bookIndexVmFactory(Rule.Book);
            Modifiers = bindableMapBuilder.Create(Rule.Modifiers);
            OpenModifierCommand = openModifierCommand;
            CreateModifierCommand = createRuleModifierCommandFactory(Rule.Modifiers);
        }

        public BookIndexViewModel Book { get; }

        public CreateRuleModifierCommand CreateModifierCommand { get; }

        public string Description
        {
            get { return Rule.DescriptionText; }
            set { Set(() => Rule.DescriptionText == value, () => Rule.DescriptionText = value); }
        }

        public IdentifierViewModel Id { get; }

        public bool IsHidden
        {
            get { return Rule.IsHidden; }
            set { Set(() => Rule.IsHidden == value, () => Rule.IsHidden = value); }
        }

        public BindableMap<ModifierFacade, IRuleModifier> Modifiers { get; }

        public string Name
        {
            get { return Rule.Name; }
            set { Set(() => Rule.Name == value, () => Rule.Name = value); }
        }

        private IRule Rule => Model;

        ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;
    }
}
