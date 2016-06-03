// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using Model;
    using Model.Repo;
    using Mvvm;

    public class DataIndexViewModel : ViewModelBase
    {
        public DataIndexViewModel(IDataIndexService dataIndexService)
        {
            DataIndexService = dataIndexService;
        }

        // TODO add open system index command

        public IObservableReadonlySet<ISystemIndex> SystemIndexes => DataIndexService.SystemIndexes;

        private IDataIndexService DataIndexService { get; }
    }
}
