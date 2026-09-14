using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class MenuService : IMenuService
{

    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;
    private readonly NavigationManager _navigation;


    public MenuService(HttpClient http, ITokenProvider tokenProvider, NavigationManager navigation)
    {
        _http = http;
        _tokenProvider = tokenProvider;
        _navigation = navigation;
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

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItemDto>>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"GetAllAsync error: {ex.Message}");
            return new List<MenuPermissionDto>();
        }
    }
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            using var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Auth.UserList);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/", true);
                return new List<UserDto>();
            }
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }


            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserDto>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Data ?? new List<UserDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAllUsersAsync error: {ex.Message}");
            return new List<UserDto>();
        }
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            using var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Menu.GetRoles);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return new List<RoleDto>();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<RoleDto>>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Data ?? new List<RoleDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAllRolesAsync error: {ex.Message}");
            return new List<RoleDto>();
        }
    }
    public async Task<SetPermissionsResponse> SetPermissionsAsync(SetPermissionsRequest request)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

public class MenuNavigationService
{
    public string? CurrentTitle { get; private set; }

    public void SetMenu(string title)
    {
        CurrentTitle = title;
    }

    public string? GetMenu()
    {
        return CurrentTitle;
    }

    public void Clear()
    {
        CurrentTitle = null;
    }
}