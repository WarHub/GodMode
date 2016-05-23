// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Views
{
    using ViewModels;

    public static class ViewModelLocator
    {
        public static AddRemoteDataSourceViewModel AddRemoteDataSourceViewModel
            => Resolve<AddRemoteDataSourceViewModel>();

        public static RemoteDataSourcesViewModel RemoteDataSourcesViewModel
            => Resolve<RemoteDataSourcesViewModel>();

        public static RemoteDataSourceIndexViewModel RemoteDataSourceIndexViewModel
            => Resolve<RemoteDataSourceIndexViewModel>();

        private static TService Resolve<TService>() where TService : class => null;
    }
}
