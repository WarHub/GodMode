// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System;
    using Demo;
    using Model;
    using Mvvm.Commands;
    using ICommand = System.Windows.Input.ICommand;

    public class IdentifierViewModel : GenericViewModel<IdentifierViewModel, IIdentifier>
    {
        private ICommand _reGenIdCommand;

        public IdentifierViewModel(IIdentifier model = null) : base(model ?? ModelLocator.Id)
        {
        }

        public string RawValue => Id.RawValue;

        public ICommand ReGenIdCommand => _reGenIdCommand ?? (_reGenIdCommand = new RelayCommand(() =>
        {
            Id.Value = Guid.NewGuid();
            RaisePropertyChanged(nameof(RawValue));
        }));

        private IIdentifier Id => Model;

        protected override IdentifierViewModel WithModelCore(IIdentifier model)
        {
            return new IdentifierViewModel(model);
        }
    }
}
