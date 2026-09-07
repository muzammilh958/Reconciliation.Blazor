using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Models.Auth;

namespace Reconciliation.Blazor.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> SignUpAsync(SignUpRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<AuthResponse> ForgetPasswordAsync(string email);
        Task<AuthResponse> ResetPasswordAsync(string email, string token, string newPassword);
        Task LogoutAsync();
        Task<string?> GetAccessTokenAsync();
        Task<string?> GetRefreshTokenAsync();
        Task SetTokensAsync(string accessToken, string refreshToken);
        Task ClearTokensAsync();
    }
    public interface ITokenProvider
    {
        Task<string?> GetAccessTokenAsync();
        Task<string?> GetRefreshTokenAsync();
        Task SetTokensAsync(string accessToken, string refreshToken);
        Task ClearTokensAsync();
    }
    public class TokenProvider : ITokenProvider
    {
        private readonly IJSRuntime _js;

        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";

        public TokenProvider(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string?> GetAccessTokenAsync()
            => await _js.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey);

        public async Task<string?> GetRefreshTokenAsync()
            => await _js.InvokeAsync<string?>("localStorage.getItem", RefreshTokenKey);

        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
            await _js.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
        }

        public async Task ClearTokensAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
        }
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";

        private readonly ITokenProvider _tokenProvider;
        private readonly AppState _appState;
        private readonly IJSRuntime _jsRuntime; 
        public AuthenticationService(HttpClient httpClient, ITokenProvider tokenProvider, AppState appState, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _appState = appState;
            _jsRuntime = jsRuntime;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(request));

                var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Auth.Login, request);

                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var raws = await response.Content.ReadAsStringAsync();

                    var results = JsonSerializer.Deserialize<AuthResponse>(
                        raws,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return results ?? new AuthResponse
                    {
                        Success = false,
                        StatusCode = (int)response.StatusCode,
                        Message = "Login failed."
                    };
                }

                var result = JsonSerializer.Deserialize<AuthResponse>(raw,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
               

                if (result?.Success == true &&
                    result.Data != null &&
                    !string.IsNullOrEmpty(result.Data.accessToken))
                {
                    var token = result.Data.accessToken;
                    var refresh = result.Data.RefreshToken ?? "";

                    await SetTokensAsync(token, refresh);
               

                    var user = result.Data.User; // MUST exist

                    if (user != null)
                    {
                        Console.WriteLine(result.Data?.User?.FirstName);
                        Console.WriteLine(result.Data?.User?.LastName);
                        Console.WriteLine(result.Data?.User?.Email);
                        _appState.SetUser(result!);
                         
                        Console.WriteLine("After SetUser");
                        Console.WriteLine(_appState.GetUser().Data?.User?.Email);
                    }
                }
                Console.WriteLine($"AuthenticationService AppState: {_appState.GetHashCode()}");
                return result ?? new AuthResponse { Success = false, Message = "Invalid response" };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<AuthResponse> SignUpAsync(SignUpRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Auth.SignUp, request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result?.Success == true && !string.IsNullOrEmpty(result.AccessToken))
                    {
                        await SetTokensAsync(result.AccessToken, result.RefreshToken ?? string.Empty);
                    }
                    return result ?? new AuthResponse { Success = false, Message = "Unknown error" };
                }

                var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return errorResult ?? new AuthResponse { Success = false, Message = "Sign-up failed" };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Success = false, Message = $"Sign-up error: {ex.Message}" };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Auth.RefreshToken, request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result?.Success == true && !string.IsNullOrEmpty(result.AccessToken))
                    {
                        await SetTokensAsync(result.AccessToken, result.RefreshToken ?? string.Empty);
                    }
                    return result ?? new AuthResponse { Success = false, Message = "Unknown error" };
                }

                return new AuthResponse { Success = false, Message = "Token refresh failed" };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Success = false, Message = $"Token refresh error: {ex.Message}" };
            }
        }

        public async Task LogoutAsync()
        {
            await ClearTokensAsync();
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await GetFromLocalStorage(AccessTokenKey);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await GetFromLocalStorage(RefreshTokenKey);
        }

        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            await SetInLocalStorage(AccessTokenKey, accessToken);
            await SetInLocalStorage(RefreshTokenKey, refreshToken);
        }

        public async Task ClearTokensAsync()
        {
            await RemoveFromLocalStorage(AccessTokenKey);
            await RemoveFromLocalStorage(RefreshTokenKey);
        }

        private async Task<string?> GetFromLocalStorage(string key)
        {
             try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            }
            catch
            {
                return null;
            }
        }

        private async Task SetInLocalStorage(string key, string value)
        {
             try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to localStorage: {ex.Message}");
            }
        }

        private async Task RemoveFromLocalStorage(string key)
        {
           try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing from localStorage: {ex.Message}");
            }
        }

        public async Task<AuthResponse> ForgetPasswordAsync(string email)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    ApiEndpoints.Auth.ForgetPassword,
                    new { Email = email }
                );

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = content
                    };
                }

                if (string.IsNullOrWhiteSpace(content))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Empty response from server"
                    };
                }

                var result = JsonSerializer.Deserialize<AuthResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return result ?? new AuthResponse
                {
                    Success = false,
                    Message = "Invalid response format"
                };
            }
            catch
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "We couldn't process request right now."
                };
            }
        }



        public async Task<AuthResponse> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var response = await _httpClient.PostAsJsonAsync(
               ApiEndpoints.Auth.ResetPassword,
                new ResetPasswordDto
                {
                    Email = email,
                    Token = token,
                    NewPassword = newPassword
                });

            return await response.Content.ReadFromJsonAsync<AuthResponse>() ?? new AuthResponse();
        }

    }


}

