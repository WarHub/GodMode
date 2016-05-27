// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Windows.Input;

    public class WrappingCommand : ICommand
    {
        private readonly object _eventLock = new object();

        public WrappingCommand(ICommand innerCommand, Action<object> preExecuteAction = null,
            Action<object> postExecuteAction = null)
        {
            if (innerCommand == null)
                throw new ArgumentNullException(nameof(innerCommand));
            InnerCommand = innerCommand;
            PreExecuteAction = preExecuteAction;
            PostExecuteAction = postExecuteAction;
        }

        private ICommand InnerCommand { get; }

        private Action<object> PostExecuteAction { get; }

        private Action<object> PreExecuteAction { get; }

        public bool CanExecute(object parameter) => InnerCommand.CanExecute(parameter);

        public void Execute(object parameter)
        {
            PreExecuteAction?.Invoke(parameter);
            InnerCommand.Execute(parameter);
            PostExecuteAction?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (_eventLock)
                {
                    if (CanExecuteChangedCore == null)
                    {
                        InnerCommand.CanExecuteChanged += InnerCommandOnCanExecuteChanged;
                    }
                    CanExecuteChangedCore += value;
                }
            }
            remove
            {
                lock (_eventLock)
                {
                    CanExecuteChangedCore -= value;
                    if (CanExecuteChangedCore == null)
                    {
                        InnerCommand.CanExecuteChanged -= InnerCommandOnCanExecuteChanged;
                    }
                }
            }
        }

        private event EventHandler CanExecuteChangedCore;

        private void InnerCommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            CanExecuteChangedCore?.Invoke(this, eventArgs);
        }
    }
}
