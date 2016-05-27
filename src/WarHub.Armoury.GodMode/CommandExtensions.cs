// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode
{
    using System;
    using Commands;
    using Mvvm.Commands;

    public static class CommandExtensions
    {
        public static ICommand SetParameter(this ICommand command, object parameter)
        {
            return new ParameterWrapperCommand(command, parameter);
        }

        public static ICommand SetParameter<T>(this ICommand<T> command, T parameter)
        {
            return new ParameterWrapperCommand(command, parameter);
        }

        public static System.Windows.Input.ICommand WrapWith(this System.Windows.Input.ICommand innerCommand,
            Action<object> preExecuteAction = null, Action<object> postExecuteAction = null)
        {
            return new WrappingCommand(innerCommand, preExecuteAction, postExecuteAction);
        }
    }
}
