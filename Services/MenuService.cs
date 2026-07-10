using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class MenuService : IMenuService
{

    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public MenuService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }

    public async Task<List<MenuItemDto>> GetAllAsync()
    {
        var response = await _http.GetAsync(ApiEndpoints.Menu.GetAll);

        response.EnsureSuccessStatusCode();
        var result = await response.Content
        .ReadFromJsonAsync<ApiResponse<List<MenuItemDto>>>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

       
        return result?.Data ?? new List<MenuItemDto>();
    }
}
