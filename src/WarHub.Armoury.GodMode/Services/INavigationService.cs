// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Services
{
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public interface INavigationService : INavigation
    {
        /// <summary>
        ///     Calls <see cref="INavigation.PushAsync(Xamarin.Forms.Page)" /> with <paramref name="bindingContext" /> set as
        ///     <paramref name="page" />'s <see cref="BindableObject.BindingContext" />.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <param name="bindingContext">Binding Context to set <paramref name="page" />'s to.</param>
        /// <returns></returns>
        Task NavigateAsync(Page page, object bindingContext);
    }
}
