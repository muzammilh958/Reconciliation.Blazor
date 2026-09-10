using System.Net.Http.Headers;
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
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            using var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Menu.GetAll);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItemDto>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Data ?? new List<MenuItemDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAllAsync error: {ex.Message}");
            return new List<MenuItemDto>();
        }
    }


    public async Task<List<MenuPermissionDto>> GetRoleMenuPermissionsAsync(string SelectedRoleId)
    {

        var token = await _tokenProvider.GetAccessTokenAsync();

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoints.Menu.GetRoleMenuPermissions}{SelectedRoleId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuPermissionDto>>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Data ?? new List<MenuPermissionDto>();

    }
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var token = await _tokenProvider.GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Auth.UserList);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserDto>>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Data ?? new List<UserDto>();
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var token = await _tokenProvider.GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Menu.GetRoles);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return new List<RoleDto>();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<RoleDto>>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Data ?? new List<RoleDto>();
    }
    public async Task<SetPermissionsResponse> SetPermissionsAsync(SetPermissionsRequest request)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            _http.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsJsonAsync(ApiEndpoints.Menu.SetPermissions, request);
            var content = await response.Content.ReadFromJsonAsync<SetPermissionsResponse>();
            return content;
        }
        catch (Exception ex)
        {
            return new SetPermissionsResponse { success = false, message = ex.Message };
        }
    }
}
