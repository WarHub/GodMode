// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using AppServices;

    public interface IAppCommandDependencyAggregate
    {
        IDialogService DialogService { get; }
    }

    public class AppCommandDependencyAggregate : IAppCommandDependencyAggregate
    {
        public AppCommandDependencyAggregate(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        public IDialogService DialogService { get; }
    }
}
