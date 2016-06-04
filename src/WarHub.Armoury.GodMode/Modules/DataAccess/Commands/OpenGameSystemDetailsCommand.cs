// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Commands
{
    using AppServices;
    using GodMode.Commands;
    using Model.Repo;
    using ViewModels;
    using Views;

    public class OpenGameSystemDetailsCommand : NavigateCommandBase<GameSystemInfo>
    {
        public OpenGameSystemDetailsCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, GameSystemDetailsVmFactory gameSystemDetailsVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            GameSystemDetailsVmFactory = gameSystemDetailsVmFactory;
        }

        private GameSystemDetailsVmFactory GameSystemDetailsVmFactory { get; }

        protected override NavTuple GetNavTuple(GameSystemInfo parameter)
        {
            var vm = GameSystemDetailsVmFactory(parameter);
            return new NavTuple(new GameSystemDetailsPage(), vm);
        }

        protected override bool CanExecuteCore(GameSystemInfo parameter) => parameter != null;
    }
}
