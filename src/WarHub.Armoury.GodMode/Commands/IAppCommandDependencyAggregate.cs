// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using AppServices;
    using Model.DataAccess;

    public interface IAppCommandDependencyAggregate
    {
        IDialogService DialogService { get; }

        ILog Log { get; }
    }

    public class AppCommandDependencyAggregate : IAppCommandDependencyAggregate
    {
        public AppCommandDependencyAggregate(IDialogService dialogService, ILog log)
        {
            DialogService = dialogService;
            Log = log;
        }

        public IDialogService DialogService { get; }

        public ILog Log { get; }
    }
}
