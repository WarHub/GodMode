// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.AppServices.Implementations
{
    using Xamarin.Forms;

    public class PageService : IPageService
    {
        private static Page StaticCurrentPage { get; set; }

        public Page CurrentPage
        {
            get { return StaticCurrentPage; }
            set { StaticCurrentPage = value; }
        }
    }
}
