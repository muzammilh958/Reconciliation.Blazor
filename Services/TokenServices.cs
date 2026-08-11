using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using Reconciliation.Blazor.Models.Auth;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class TokenServices
{
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private const string UserInfoKey = "userInfo";

    public TokenServices(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveToken(string token)
    {
        await _localStorage.SetItemAsync(TokenKey, token);
    }

    public async Task<AuthResponse?> GetUserInfo()
    {
        try
        {
            var userInfoString = await _localStorage.GetItemAsync(UserInfoKey);
            if (string.IsNullOrEmpty(userInfoString))
            {
                Console.WriteLine("No userInfo found in localStorage");
                return null;
            }

            // Log the raw JSON for debugging
            Console.WriteLine($"Raw userInfo JSON: {userInfoString}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = JsonSerializer.Deserialize<AuthResponse>(userInfoString, options);

            if (result == null)
            {
                Console.WriteLine("Deserialized userInfo but result is null");
                return null;
            }

            return result;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON deserialization error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error in GetUserInfo: {ex.Message}");
            return null;
        }
    }

    public async Task ClearUserInfo()
    {
        await _localStorage.RemoveItemAsync(UserInfoKey);
        await _localStorage.RemoveItemAsync(TokenKey);
    }

    public async Task SaveTokenAndUserInfo(AuthResponse authResponse)
    {
        if (authResponse == null)
            throw new ArgumentNullException(nameof(authResponse));

        if (authResponse.Data == null)
            throw new ArgumentException("Response data is null");

        var token = authResponse.Data.accessToken;
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("No token found in response");

        // Save token
        await _localStorage.SetItemAsync(TokenKey, token);

        // Save user info if available
        if (authResponse.Data.User != null)
        {
            var userJson = JsonSerializer.Serialize(authResponse);
            Console.WriteLine($"Saving user info: {userJson}");
            await _localStorage.SetItemAsync(UserInfoKey, userJson);
        }
    }

    public async Task<string> GetToken()
    {
        var result = await _localStorage.GetItemAsync(TokenKey);
        return result ?? string.Empty;
    }

   
    public Task RemoveToken() => _localStorage.RemoveItemAsync(TokenKey);

    public async Task<bool> IsTokenExists()
    {
        var token = await GetToken();
        return !string.IsNullOrEmpty(token);
    }
}