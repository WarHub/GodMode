// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using Mvvm.Commands;

    public class ParameterWrapperCommand : ICommand
    {
        private readonly object _objectLock = new object();

        public ParameterWrapperCommand(System.Windows.Input.ICommand wrappedCommand, object parameter)
        {
            if (wrappedCommand == null)
                throw new ArgumentNullException(nameof(wrappedCommand));
            WrappedCommand = wrappedCommand;
            Parameter = parameter;
        }

        private object Parameter { get; }

        private System.Windows.Input.ICommand WrappedCommand { get; }

        public bool CanExecute(object parameter) => WrappedCommand.CanExecute(Parameter);

        public void Execute(object parameter) => WrappedCommand.Execute(Parameter);

        public bool CanExecute() => WrappedCommand.CanExecute(Parameter);

        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (_objectLock)
                {
                    if (CanExecuteChangedCore == null)
                    {
                        WrappedCommand.CanExecuteChanged += OnWrappedCommandCanExecuteChanged;
                    }
                    CanExecuteChangedCore += value;
                }
            }
            remove
            {
                lock (_objectLock)
                {
                    CanExecuteChangedCore -= value;
                    if (CanExecuteChangedCore == null)
                    {
                        WrappedCommand.CanExecuteChanged -= OnWrappedCommandCanExecuteChanged;
                    }
                }
            }
        }

        public void Execute() => WrappedCommand.Execute(Parameter);

        private event EventHandler CanExecuteChangedCore;

        private void OnWrappedCommandCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChangedCore?.Invoke(this, e);
        }
    }
}
