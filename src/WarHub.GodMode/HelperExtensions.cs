using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WarHub.GodMode
{
    public static class HelperExtensions
    {
        public static ValueTask Focus(this IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeVoidAsync("hackaroundFocus", element);
        }
    }
}
