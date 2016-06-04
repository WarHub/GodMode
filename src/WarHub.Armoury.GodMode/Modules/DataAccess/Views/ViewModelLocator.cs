// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Views
{
    using ViewModels;

    public static class ViewModelLocator
    {
        public static AddRemoteDataSourceViewModel AddRemoteDataSourceViewModel
            => Resolve<AddRemoteDataSourceViewModel>();

        public static CatalogueDetailsViewModel CatalogueDetailsViewModel => Resolve<CatalogueDetailsViewModel>();

        public static DataIndexViewModel DataIndexViewModel => Resolve<DataIndexViewModel>();

        public static GameSystemDetailsViewModel GameSystemDetailsViewModel => Resolve<GameSystemDetailsViewModel>();

        public static RemoteDataSourceIndexViewModel RemoteDataSourceIndexViewModel
            => Resolve<RemoteDataSourceIndexViewModel>();

        public static RemoteDataSourcesViewModel RemoteDataSourcesViewModel
            => Resolve<RemoteDataSourcesViewModel>();

        public static SystemIndexViewModel SystemIndexViewModel => Resolve<SystemIndexViewModel>();

        private static TService Resolve<TService>() where TService : class => null;
    }
}
