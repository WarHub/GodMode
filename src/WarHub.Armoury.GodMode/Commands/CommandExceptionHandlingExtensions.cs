// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;

    public static class CommandExceptionHandlingExtensions
    {
        public static void HandleException(this IAppCommandDependencyAggregate dependencyAggregate, Exception exception)
        {
            dependencyAggregate.Log.Warn?.With("Command failed.", exception);
            dependencyAggregate.DialogService.ShowDialogAsync("Operation failed", exception.Message, "oh well");
        }
    }
}
