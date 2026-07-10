using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Reconciliation.Blazor.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync(AccessTokenKey);
                
                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = GetClaimsFromToken(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsAuthenticated(string token, string refreshToken)
        {
            try
            {
                await _localStorage.SetItemAsync(AccessTokenKey, token);
                await _localStorage.SetItemAsync(RefreshTokenKey, refreshToken);

                var claims = GetClaimsFromToken(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to authenticate user: {ex.Message}", ex);
            }
        }

        public async Task MarkUserAsLoggedOut()
        {
            try
            {
                await _localStorage.RemoveItemAsync(AccessTokenKey);
                await _localStorage.RemoveItemAsync(RefreshTokenKey);

                var identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to log out user: {ex.Message}", ex);
            }
        }

        private List<Claim> GetClaimsFromToken(string token)
        {
            var claims = new List<Claim>();

            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                
                if (!jwtHandler.CanReadToken(token))
                {
                    return claims;
                }

                var jwtToken = jwtHandler.ReadJwtToken(token);

                // Add standard claims
                foreach (var claim in jwtToken.Claims)
                {
                    claims.Add(new Claim(claim.Type, claim.Value));
                }

                // Ensure common identity claims are present
                if (!claims.Any(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    var subClaim = claims.FirstOrDefault(c => c.Type == "sub");
                    if (subClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, subClaim.Value));
                    }
                }

                if (!claims.Any(c => c.Type == ClaimTypes.Name))
                {
                    var emailClaim = claims.FirstOrDefault(c => c.Type == "email");
                    if (emailClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, emailClaim.Value));
                    }
                }

                // Add role claims if available
                var roleClaims = claims.Where(c => c.Type == "role").ToList();
                foreach (var roleClaim in roleClaims)
                {
                    if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == roleClaim.Value))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
                    }
                }

                return claims;
            }
            catch (Exception)
            {
                return claims;
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _localStorage.GetItemAsync(AccessTokenKey);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await _localStorage.GetItemAsync(RefreshTokenKey);
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                
                if (!jwtHandler.CanReadToken(token))
                {
                    return true;
                }

                var jwtToken = jwtHandler.ReadJwtToken(token);
                var expirationClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");

                if (expirationClaim == null || !long.TryParse(expirationClaim.Value, out var unixTime))
                {
                    return false;
                }

                var expirationDateTime = UnixTimeStampToDateTime(unixTime);
                return DateTime.UtcNow >= expirationDateTime;
            }
            catch
            {
                return true;
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTime;
        }
    }
}
