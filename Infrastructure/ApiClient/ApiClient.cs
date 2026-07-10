using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Interfaces;

namespace Reconciliation.Blazor.Infrastructure.ApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url)
        {
            return await _httpClient.GetFromJsonAsync<TResponse>(url);
        }
    }
}