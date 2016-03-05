// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class NavigationService : INavigationService
    {
        public NavigationService(IPageService pageService)
        {
            PageService = pageService;
        }

        private INavigation Base
        {
            get
            {
                var @base = Application.Current.MainPage.Navigation;
                if (@base == null)
                {
                    throw new InvalidOperationException(
                        $"Cannot use {nameof(NavigationService)} without navigable {nameof(Application.Current.MainPage)}.");
                }
                return @base;
            }
        }

        private IPageService PageService { get; }

        public void RemovePage(Page page)
        {
            Base.RemovePage(page);
            UpdateCurrentPage();
        }

        public void InsertPageBefore(Page page, Page before) => Base.InsertPageBefore(page, before);

        public async Task PushAsync(Page page)
        {
            await Base.PushAsync(page);
            UpdateCurrentPage();
        }

        public async Task<Page> PopAsync()
        {
            var page = await Base.PopAsync();
            UpdateCurrentPage();
            return page;
        }

        public async Task PopToRootAsync()
        {
            await Base.PopToRootAsync();
            UpdateCurrentPage();
        }

        public async Task PushModalAsync(Page page)
        {
            await Base.PushModalAsync(page);
            UpdateCurrentPage();
        }

        public async Task<Page> PopModalAsync()
        {
            var page = await Base.PopModalAsync();
            UpdateCurrentPage();
            return page;
        }

        public async Task PushAsync(Page page, bool animated)
        {
            await Base.PushAsync(page, animated);
            UpdateCurrentPage();
        }

        public async Task<Page> PopAsync(bool animated)
        {
            var page = await Base.PopAsync(animated);
            UpdateCurrentPage();
            return page;
        }

        public async Task PopToRootAsync(bool animated)
        {
            await Base.PopToRootAsync(animated);
            UpdateCurrentPage();
        }

        public async Task PushModalAsync(Page page, bool animated)
        {
            await Base.PushModalAsync(page, animated);
            UpdateCurrentPage();
        }

        public async Task<Page> PopModalAsync(bool animated)
        {
            var page = await Base.PopModalAsync(animated);
            UpdateCurrentPage();
            return page;
        }

        public IReadOnlyList<Page> NavigationStack => Base.NavigationStack;

        public IReadOnlyList<Page> ModalStack => Base.ModalStack;

        public async Task NavigateAsync(Page page, object bindingContext)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            page.BindingContext = null;
            page.BindingContext = bindingContext;
            await PushAsync(page);
        }

        private void UpdateCurrentPage()
        {
            PageService.CurrentPage = ModalStack.Count > 0 ? ModalStack.Last() : NavigationStack.Last();
        }
    }
}
