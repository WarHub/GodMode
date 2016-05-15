// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Home.Views
{
    using ViewModels;

    /// <summary>
    ///     Returns null, used only for BindingContext typing in XAML views.
    /// </summary>
    public static class ViewModelLocator
    {
        public static MainViewModel MainViewModel => Resolve<MainViewModel>();

        private static TService Resolve<TService>() where TService : class => null;
    }
}
