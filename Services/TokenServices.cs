using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;

namespace Reconciliation.Blazor;

public class TokenServices
{
    private readonly IJSRuntime _js;

    private const string Key = "authToken";

    public TokenServices(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SaveToken(string token)
        => await _js.InvokeVoidAsync("localStorage.setItem", Key, token);

    public async Task<string?> GetToken()
        => await _js.InvokeAsync<string?>("localStorage.getItem", Key);

    public async Task ClearToken()
        => await _js.InvokeVoidAsync("localStorage.removeItem", Key);



    public bool IsTokenExpired(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return true;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.ValidTo <= DateTime.UtcNow;
    }
}
