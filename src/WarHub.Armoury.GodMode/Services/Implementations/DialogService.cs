// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Services.Implementations
{
    using System.Threading.Tasks;

    public class DialogService : IDialogService
    {
        public DialogService(IPageService pageService)
        {
            PageService = pageService;
        }

        private IPageService PageService { get; }

        public async Task ShowDialogAsync(string title, string message, string cancel)
        {
            await PageService.CurrentPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> ShowDialogAsync(string title, string message, string accept, string cancel)
        {
            return await PageService.CurrentPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> ShowOptionsAsync(string title, string cancel, string destruction,
            params string[] buttons)
        {
            return await PageService.CurrentPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }
    }
}
