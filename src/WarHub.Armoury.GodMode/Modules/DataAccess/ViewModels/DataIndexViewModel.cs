// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using Commands;
    using Model;
    using Model.Repo;
    using Mvvm;

    public class DataIndexViewModel : ViewModelBase
    {
        public DataIndexViewModel(IDataIndexService dataIndexService, RemoveSystemCommand removeSystemCommand,
            RefreshDataIndexCommand refreshDataIndexCommand, OpenSystemIndexCommand openSystemIndexCommand)
        {
            DataIndexService = dataIndexService;
            RemoveSystemCommand = removeSystemCommand;
            RefreshDataIndexCommand = refreshDataIndexCommand;
            OpenSystemIndexCommand = openSystemIndexCommand;
        }

        public OpenSystemIndexCommand OpenSystemIndexCommand { get; }

        public RefreshDataIndexCommand RefreshDataIndexCommand { get; }

        public RemoveSystemCommand RemoveSystemCommand { get; }

        public IObservableReadonlySet<ISystemIndex> SystemIndexes => DataIndexService.SystemIndexes;

        private IDataIndexService DataIndexService { get; }
    }
}
