// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Services
{
    using System.Threading.Tasks;

    public interface IDialogService
    {
        Task ShowDialogAsync(string title, string message, string cancel);
        Task<bool> ShowDialogAsync(string title, string message, string accept, string cancel);
        Task<string> ShowOptionsAsync(string title, string cancel, string destruction, params string[] buttons);
    }
}
