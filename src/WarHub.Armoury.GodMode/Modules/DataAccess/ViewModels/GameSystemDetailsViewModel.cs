// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using Model.Repo;
    using Mvvm;

    public delegate GameSystemDetailsViewModel GameSystemDetailsVmFactory(GameSystemInfo gameSystemInfo);

    public class GameSystemDetailsViewModel : ViewModelBase
    {
        public GameSystemDetailsViewModel(GameSystemInfo gameSystemInfo)
        {
            GameSystemInfo = gameSystemInfo;
        }

        public GameSystemInfo GameSystemInfo { get; }
    }
}
