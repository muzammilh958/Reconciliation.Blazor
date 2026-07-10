using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace Reconciliation.Blazor;

public class ApiAuthHandler : DelegatingHandler
{
    private readonly TokenServices _tokenService;
    private readonly NavigationManager _nav;

    public ApiAuthHandler(TokenServices tokenService, NavigationManager nav)
    {
        _tokenService = tokenService;
        _nav = nav;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            if (JwtParser.GetExpiry(token) < DateTime.UtcNow)
            {
                await _tokenService.ClearToken();
                _nav.NavigateTo("/login", true);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _tokenService.ClearToken();
            _nav.NavigateTo("/login", true);
        }

        return response;
    }

}
