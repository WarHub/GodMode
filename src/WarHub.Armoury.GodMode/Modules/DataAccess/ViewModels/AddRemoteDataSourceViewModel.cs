// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.ViewModels
{
    using System.Windows.Input;
    using Commands;
    using Mvvm;

    public class AddRemoteDataSourceViewModel : ViewModelBase
    {
        private string _name;
        private string _url;

        public AddRemoteDataSourceViewModel(AddDataSourceCommand addDataSourceCommand)
        {
            AddDataSourceCommand = addDataSourceCommand;
        }

        public ICommand AddDataSourceCommand { get; }

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }
    }
}
