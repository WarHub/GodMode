// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Bindables;
    using Demo;
    using Model;
    using Models;
    using Services;

    public class ProfileViewModel : GenericViewModel<ProfileViewModel, IProfile>, IModifiersListViewModel
    {
        private IReadOnlyList<CharacteristicViewModel> _characteristics;

        public ProfileViewModel(ICommandsAggregateService commands, IProfile model = null)
            : base(model ?? ModelLocator.Profile)
        {
            Commands = commands;
            Id = ViewModelLocator.IdentifierViewModel.WithModel(Profile.Id);
            Book = ViewModelLocator.BookIndexViewModel.WithModel(Profile.Book);
            Modifiers =
                Profile.Modifiers.ToBindableMap(removeCommand: Commands.RemoveModifierCommand.For(() => Modifiers));
            UpdateCharacteristicViewModels();
        }

        public BookIndexViewModel Book { get; }

        public IReadOnlyList<CharacteristicViewModel> Characteristics
        {
            get { return _characteristics; }
            private set { Set(ref _characteristics, value); }
        }

        public IdentifierViewModel Id { get; }

        public bool IsHidden
        {
            get { return Profile.IsHidden; }
            set { Set(() => Profile.IsHidden == value, () => Profile.IsHidden = value); }
        }

        public BindableMap<ModifierFacade, IProfileModifier> Modifiers { get; }

        public string Name
        {
            get { return Profile.Name; }
            set { Set(() => Profile.Name == value, () => Profile.Name = value); }
        }

        public IProfileType ProfileType
        {
            get { return Profile.TypeLink.Target; }
            set
            {
                if (Set(() => Profile.TypeLink.Target == value, () => Profile.TypeLink.Target = value))
                {
                    UpdateModelCharacteristics();
                }
            }
        }

        public IEnumerable<IProfileType> ProfileTypes => Profile.Context.Catalogue.SystemContext.ProfileTypes;

        private ICommandsAggregateService Commands { get; }

        private IProfile Profile => Model;

        public ICommand CreateModifierCommand => Commands.CreateModifierCommand.EnableFor(Profile.Modifiers);

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

        public ICommand OpenModifierCommand => Commands.OpenModifierCommand;

        private void UpdateModelCharacteristics()
        {
            Profile.Characteristics.Clear();
            foreach (var characteristicType in ProfileType.CharacteristicTypes)
            {
                var characteristic = Profile.Characteristics.AddNew();
                characteristic.Name = characteristicType.Name;
                characteristic.TypeId.Value = characteristicType.Id.Value;
            }
            UpdateCharacteristicViewModels();
        }

        private void UpdateCharacteristicViewModels()
        {
            var characteristicVm = ViewModelLocator.CharacteristicViewModel;
            Characteristics =
                Profile.Characteristics.Select(characteristic => characteristicVm.WithModel(characteristic)).ToList();
        }

        protected override ProfileViewModel WithModelCore(IProfile model)
        {
            return new ProfileViewModel(Commands, model);
        }
    }
}
