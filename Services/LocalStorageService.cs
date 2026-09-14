using Microsoft.JSInterop;

namespace Reconciliation.Blazor.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string?> GetItemAsync(string key)
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SetItemAsync(string key, string value)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
            }
            catch (Exception)
            {
                // Silently fail if localStorage is not available
            }
        }

        public async Task RemoveItemAsync(string key)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item from localStorage: {ex.Message}");
               
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.clear");
            }
            catch (Exception)
            {
                // Silently fail if localStorage is not available
            }
        }
    }
}
