// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using Demo;
    using Model;

    public class CharacteristicViewModel : GenericViewModel<CharacteristicViewModel, ICharacteristic>
    {
        public CharacteristicViewModel(ICharacteristic model = null) : base(model ?? ModelLocator.Characteristic)
        {
        }

        public string Name => Characteristic.Name;

        public string Value
        {
            get { return Characteristic.Value; }
            set { Set(() => Characteristic.Value == value, () => Characteristic.Value = value); }
        }

        private ICharacteristic Characteristic => Model;

        protected override CharacteristicViewModel WithModelCore(ICharacteristic model)
        {
            return new CharacteristicViewModel(model);
        }
    }
}
