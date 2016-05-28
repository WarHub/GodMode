// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using AppServices;
    using Bindables;
    using Commands;
    using Model;
    using Models;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class ProfileModifierViewModel :
        ModifierViewModelBase<IProfileModifier, string, ProfileModifierAction, IIdentifier>
    {
        public ProfileModifierViewModel(IProfileModifier model,
            CreateConditionItemCommandFactory createConditionItemCommandFactory,
            OpenConditionItemCommand openConditionItemCommand,
            Func<IBindableMap<ConditionItemFacade>, RemoveConditionItemCommand> removeCommandFactory,
            IDialogService dialogService)
            : base(model, createConditionItemCommandFactory, openConditionItemCommand, removeCommandFactory)
        {
            DialogService = dialogService;
            ParentProfile = Modifier.GetParentProfile();
            CharacteristicTypes = ParentProfile.TypeLink.Target.CharacteristicTypes;
            Fields = CharacteristicTypes.Select(type => type.Id).ToArray();
            IsFieldActive = CanFieldBeActive();
            IsValueActive = CanValueBeActive();
        }

        public override IEnumerable<ProfileModifierAction> Actions
        {
            get
            {
                yield return ProfileModifierAction.Increment;
                yield return ProfileModifierAction.Decrement;
                yield return ProfileModifierAction.Append;
                yield return ProfileModifierAction.Hide;
                yield return ProfileModifierAction.Show;
                yield return ProfileModifierAction.Set;
            }
        }

        public override IEnumerable<IIdentifier> Fields { get; }

        public ICharacteristicType CharacteristicType
        {
            get { return CharacteristicTypes.FirstOrDefault(type => type.Id.Value == Field.Value); }
            set
            {
                if (Set(() => Field.Value == value.Id.Value, () => Field.Value = value.Id.Value))
                {
                    OnCharacteristicTypeChanged();
                }
            }
        }

        public IEnumerable<ICharacteristicType> CharacteristicTypes { get; }

        private IDialogService DialogService { get; }

        private IProfile ParentProfile { get; }

        private void OnCharacteristicTypeChanged()
        {
            ValidateValueForActionAndCharacteristicValue();
        }

        private bool CanFieldBeActive() => Action != ProfileModifierAction.Hide && Action != ProfileModifierAction.Show;

        private bool CanValueBeActive() => CanFieldBeActive();

        protected override void OnActionChanged()
        {
            base.OnActionChanged();
            IsFieldActive = CanFieldBeActive();
            IsValueActive = CanValueBeActive();
            ValidateValueForActionAndCharacteristicValue();
        }

        protected override void OnFieldChanged()
        {
            base.OnFieldChanged();
            ValidateValueForActionAndCharacteristicValue();
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            ValidateValueForActionAndCharacteristicValue();
        }

        private async void ValidateValueForActionAndCharacteristicValue()
        {
            if (Action != ProfileModifierAction.Increment && Action != ProfileModifierAction.Decrement)
            {
                return;
            }
            var targetCharacteristic =
                ParentProfile.Characteristics.First(characteristic => characteristic.TypeId.Value == Field.Value);
            var profileValue = targetCharacteristic.Value;
            decimal decimalResult;
            var canModifierValueParse = decimal.TryParse(Value, out decimalResult);
            var canProfileValueParse = decimal.TryParse(profileValue, out decimalResult);
            if (canModifierValueParse && canProfileValueParse)
            {
                return;
            }
            await AlertInvalidActionValueAsync(profileValue, Value, targetCharacteristic.Name);
            Value = null;
        }

        private async Task AlertInvalidActionValueAsync(string profileValue, string modifierValue,
            string characteristicName)
        {
            await DialogService.ShowDialogAsync("Invalid value",
                $"If {nameof(Action)} is either {nameof(ProfileModifierAction.Increment)}" +
                $" or {nameof(ProfileModifierAction.Decrement)}," +
                $" '{ParentProfile.Name}' profile's '{characteristicName}' value ('{profileValue}')" +
                $" and modifier value ('{modifierValue}') both must be numbers.",
                "okey");
        }
    }
}
