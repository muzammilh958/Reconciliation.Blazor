using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public interface IUserService
{
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetCurrentUserAsync();
    Task<List<RolesDto>?> GetRolesAsync();

    Task<bool> UpdateUserAsync(UserDto request);
    Task<bool> DeleteUserAsync(Guid id);
    Task<ApiResponse<UserCreateResult>> CreateUserAsync(CreateUserDto model);
}
public class UserAdminService : IUserService
{
    private readonly HttpClient _http;
    private readonly ITokenProvider _tokenProvider;

    private readonly IAuthenticationService _authService;
    public UserAdminService(HttpClient http, IAuthenticationService authService, ITokenProvider tokenProvider)
    {
        _http = http;
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    public async Task<ApiResponse<UserCreateResult>> CreateUserAsync(CreateUserDto model)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it

            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Auth.UserCreate);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error creating user: {json}");

                var messages = new List<string>();

                try
                {
                    using var doc = JsonDocument.Parse(json);

                      if (doc.RootElement.TryGetProperty("message", out var messageElement))
                        {
                            // Get the message directly
                            var errorMessage = messageElement.GetString();
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                messages.Add(errorMessage);
                            }
                        }
                    if (doc.RootElement.TryGetProperty("errors", out var errors))
                    {
                        foreach (var field in errors.EnumerateObject())
                        {
                            foreach (var message in field.Value.EnumerateArray())
                            {
                                messages.Add(message.GetString()!);
                            }
                        }
                    }
                }
                catch
                {
                    messages.Add("Something went wrong.");
                }

                return new ApiResponse<UserCreateResult>
                {
                    Success = false,
                    Message = string.Join("\n", messages),
                    Errors = messages
                };
                // throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            ApiResponse<UserCreateResult>? result = await response.Content.ReadFromJsonAsync<ApiResponse<UserCreateResult>>();

            return result ?? new ApiResponse<UserCreateResult>
            {
                Success = false,
                Message = "Empty response from server",
                Data = null
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
            return new ApiResponse<UserCreateResult>
            {
                Success = false,
                Message = "Error creating user",
                Data = null
            };
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiEndpoints.Auth.UserDelete}/{id}");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            Console.WriteLine($"Delete User Response: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
            return false;
        }
    }

    public Task<UserDto?> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<RolesDto>?> GetRolesAsync()
    {
        var token = await _tokenProvider.GetAccessTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Auth.RoleList);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<RolesDto>>>();

        return result?.Data ?? new List<RolesDto>();
    }

    public Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var token = await _tokenProvider.GetAccessTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Auth.UserList);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserDto>>>();

        return result?.Data ?? new List<UserDto>();
    }

    public async Task<bool> UpdateUserAsync(UserDto user)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoints.Auth.UserUpdate + user.Id);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(user);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserUpdateResponse>>();

        return result?.Success ?? false;
    }
}
