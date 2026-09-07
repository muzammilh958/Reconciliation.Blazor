using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Models.Auth;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenServices _tokenServices;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private AuthenticationState? _cachedState;

        private readonly Dictionary<string, (bool IsValid, DateTime CachedAt)> _tokenCache = new Dictionary<string, (bool, DateTime)>();
        private readonly TimeSpan _cacheDuration = TimeSpan.FromSeconds(30);


        public JwtAuthStateProvider(TokenServices tokenServices,
         ILocalStorageService localStorage,
        HttpClient httpClient,
        NavigationManager navigationManager)
        {
            _tokenServices = tokenServices;
            _localStorage = localStorage;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_cachedState != null)
                return _cachedState;

            try
            {
                var token = await _tokenServices.GetToken();
               
                if (string.IsNullOrEmpty(token) || !IsTokenValid(token))
                {
                    _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    return _cachedState;
                }

                if (!IsTokenValid(token))
                {
                    await ClearInvalidToken();
                    _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    return _cachedState;
                }

                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                _cachedState = new AuthenticationState(user);
                return _cachedState;
            }
            catch
            {
                _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                return _cachedState;
            }
        }


        private async Task<bool> ValidateTokenWithApi(string token)
        {
            try
            {

                if (IsTokenExpiredLocally(token))
                {
                    return false; // Token already expired, no need to call API
                }

                if (_tokenCache.TryGetValue(token, out var cached)
           && DateTime.UtcNow - cached.CachedAt < _cacheDuration)
                {
                    return cached.IsValid;
                }
                // Authorization header set karein
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var response = await _httpClient.GetAsync(
                    ApiEndpoints.Auth.ValidateToken,
                    cts.Token
                );
                bool isValid = response.IsSuccessStatusCode;

                // Agar 200 OK aaya toh valid
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<ValidateTokenResponse>();
                    return content?.valid == true;
                }
                _tokenCache[token] = (isValid, DateTime.UtcNow);
                return isValid;
            }
            catch (TaskCanceledException)
            {
                // Timeout - clear token
                await ClearInvalidToken();
                return false;
            }
            catch
            {
                await ClearInvalidToken();
                return false;
            }
        }

        private bool IsTokenExpiredLocally(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    return jwtToken.ValidTo < DateTime.UtcNow;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task ClearInvalidToken()
        {
            await _tokenServices.RemoveToken();
            await _localStorage.RemoveItemAsync("userInfo");
            _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));

            // Login page pe bhejein
            _navigationManager.NavigateTo("/", true);
        }


        public async Task NotifyUserAuthentication(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task NotifyUserLogout()
        {
            await _tokenServices.RemoveToken();
            await _localStorage.RemoveItemAsync("userInfo");
            _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));
        }

        private bool IsTokenValid(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.ValidTo > DateTime.UtcNow.AddMinutes(-1);
            }
            catch
            {
                return false;
            }
        }

        private List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwt);
                return jwtToken.Claims.ToList();
            }
            catch
            {
                return claims;
            }
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}