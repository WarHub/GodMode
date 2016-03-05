// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System.Windows.Input;
    using Demo;
    using Model;
    using ModelFacades;
    using Models;
    using Services;

    public class RuleViewModel : GenericViewModel<RuleViewModel, IRule>, IModifiersListViewModel
    {
        public RuleViewModel(ICommandsAggregateService commands, IRule rule = null)
            : base(rule ?? ModelLocator.Rule)
        {
            Commands = commands;
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Rule.Id);
            Book = ViewModelLocator.BookIndexViewModel.WithModel(Rule.Book);
            Modifiers = Rule.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
        }

        public BookIndexViewModel Book { get; }

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

        private ICommandsAggregateService Commands { get; }

        private IRule Rule => Model;

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Rule.Modifiers);

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

        protected override RuleViewModel WithModelCore(IRule model)
        {
            return new RuleViewModel(Commands, model);
        }
    }
}
