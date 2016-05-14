// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.ViewModels
{
    using Autofac;

    public static class ViewModelLocator
    {
        public static MainViewModel MainViewModel => Resolve<MainViewModel>();

        private static TService Resolve<TService>() => App.ServiceProvider.Resolve<TService>();
    }
}
