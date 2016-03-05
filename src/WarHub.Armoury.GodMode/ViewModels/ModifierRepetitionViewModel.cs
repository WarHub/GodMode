// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using Model;
    using Mvvm;

    public class ModifierRepetitionViewModel : ViewModelBase
    {
        public ModifierRepetitionViewModel(IRepetitionInfo repetition)
        {
            Repetition = repetition;
        }

        public bool IsActive
        {
            get { return Repetition.IsActive; }
            set { Set(() => Repetition.IsActive == value, () => Repetition.IsActive = value); }
        }

        public int Loops
        {
            get { return (int) Repetition.Loops; }
            set
            {
                var uintLoops = (uint) value;
                Set(() => Repetition.Loops == uintLoops, () => Repetition.Loops = uintLoops);
            }
        }

        //TODO the rest of repetition properties (children/parent)

        private IRepetitionInfo Repetition { get; }
    }
}
