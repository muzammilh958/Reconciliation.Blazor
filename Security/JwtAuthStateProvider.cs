using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
        {
            private readonly TokenServices _tokenServices;
            private readonly ILocalStorageService _localStorage;

            public JwtAuthStateProvider(TokenServices tokenServices, ILocalStorageService localStorage)
            {
                _tokenServices = tokenServices;
                _localStorage = localStorage;
            }

            public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                try
                {
                    var token = await _tokenServices.GetToken();

                    if (string.IsNullOrEmpty(token) || !IsTokenValid(token))
                    {
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }

                    var claims = ParseClaimsFromJwt(token);
                    var identity = new ClaimsIdentity(claims, "jwt");
                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }
                catch
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
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
                var anonymousUser = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                NotifyAuthenticationStateChanged(Task.FromResult(anonymousUser));
            }

            private bool IsTokenValid(string token)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    return jwtToken.ValidTo > DateTime.UtcNow;
                }
                catch
                {
                    return false;
                }
            }

            private List<Claim> ParseClaimsFromJwt(string jwt)
            {
                var claims = new List<Claim>();
                var payload = jwt.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                if (keyValuePairs != null)
                {
                    foreach (var kvp in keyValuePairs)
                    {
                        if (kvp.Key == "role" || kvp.Key == "roles")
                        {
                            if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var item in element.EnumerateArray())
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, item.GetString()!));
                                }
                            }
                            else
                            {
                                claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString()!));
                            }
                        }
                        else
                        {
                            claims.Add(new Claim(kvp.Key, kvp.Value.ToString()!));
                        }
                    }
                }

                return claims;
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