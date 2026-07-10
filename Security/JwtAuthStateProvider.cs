using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Reconciliation.Blazor;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly TokenServices _tokenService;

    public JwtAuthStateProvider(TokenServices tokenService)
    {
        _tokenService = tokenService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            return Anonymous();
        }

        if (_tokenService.IsTokenExpired(token))
        {
            await _tokenService.ClearToken();
            return Anonymous();
        }
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = jwt.Claims.ToList();

        var identity = new ClaimsIdentity(claims, "jwt");

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    private IEnumerable<Claim> ParseClaims(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        return token.Claims;
    }
    public async Task NotifyUserAuthentication(string jwt)
    {
        await _tokenService.SaveToken(jwt);
        var identity = new ClaimsIdentity(ParseClaims(jwt), "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task NotifyUserLogout()
    {
        await _tokenService.ClearToken();

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(anonymous)));
    }

    private AuthenticationState Anonymous()
    {
        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity()));
    }
    public void MarkUserAsAuthenticated(string token)
    {
        var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }
}
