// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
    public class ProfileViewModel : GenericViewModel<IProfile>, IModifiersListViewModel
    {
        private IReadOnlyList<CharacteristicViewModel> _characteristics;

        public ProfileViewModel(IProfile model,
            Func<IIdentifier, IdentifierViewModel> identifierVmFactory,
            Func<IBookIndex, BookIndexViewModel> bookIndexVmFactory,
            Func<ICharacteristic, CharacteristicViewModel> characteristicVmFactory,
            BindableMapBuilder bindableMapBuilder,
            OpenModifierCommand openModifierCommand,
            CreateProfileModifierCommandFactory createProfileModifierCommandFactory)
            : base(model)
        {
            Id = identifierVmFactory(Profile.Id);
            Book = bookIndexVmFactory(Profile.Book);
            Modifiers = bindableMapBuilder.Create(Profile.Modifiers);
            CharacteristicVmFactory = characteristicVmFactory;
            OpenModifierCommand = openModifierCommand;
            CreateModifierCommand = createProfileModifierCommandFactory(Profile.Modifiers);
            UpdateCharacteristicViewModels();
        }

        public BookIndexViewModel Book { get; }

        public IReadOnlyList<CharacteristicViewModel> Characteristics
        {
            get { return _characteristics; }
            private set { Set(ref _characteristics, value); }
        }

        public CreateProfileModifierCommand CreateModifierCommand { get; }

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

        private Func<ICharacteristic, CharacteristicViewModel> CharacteristicVmFactory { get; }

        private IProfile Profile => Model;

        ICommand IModifiersListViewModel.CreateModifierCommand => CreateModifierCommand;

        public OpenModifierCommand OpenModifierCommand { get; }

        IBindableGrouping<ModifierFacade> IModifiersListViewModel.Modifiers => Modifiers;

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
            Characteristics =
                Profile.Characteristics.Select(characteristic => CharacteristicVmFactory(characteristic)).ToList();
        }
    }
}
