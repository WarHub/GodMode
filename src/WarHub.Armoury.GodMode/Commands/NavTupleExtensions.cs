// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System.Threading.Tasks;
    using AppServices;

    public static class NavTupleExtensions
    {
        public static async Task NavigateAsync(this INavigationService navigationService, NavTuple navTuple)
        {
            await navigationService.NavigateAsync(navTuple.Page, navTuple.ViewModel);
        }
    }
}
