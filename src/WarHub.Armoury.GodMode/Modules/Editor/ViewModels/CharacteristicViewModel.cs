// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using Model;

    public class CharacteristicViewModel : GenericViewModel<ICharacteristic>
    {
        public CharacteristicViewModel(ICharacteristic model) : base(model)
        {
        }

        public string Name => Characteristic.Name;

        public string Value
        {
            get { return Characteristic.Value; }
            set { Set(() => Characteristic.Value == value, () => Characteristic.Value = value); }
        }

        private ICharacteristic Characteristic => Model;
    }
}
