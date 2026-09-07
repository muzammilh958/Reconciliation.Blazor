using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
namespace Reconciliation.Blazor;

public class ApiAuthHandler  : DelegatingHandler
{
        private readonly TokenServices _tokenServices;
        private readonly NavigationManager _navigationManager;
        private readonly JwtAuthStateProvider _authStateProvider;

        public ApiAuthHandler(
            TokenServices tokenServices,
            NavigationManager navigationManager,
            JwtAuthStateProvider authStateProvider)
        {
            _tokenServices = tokenServices;
            _navigationManager = navigationManager;
            _authStateProvider = authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Token add karein har request mein
            var token = await _tokenServices.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Request send karein
            var response = await base.SendAsync(request, cancellationToken);

            // Agar 401 Unauthorized aaya toh token clear karein
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _tokenServices.RemoveToken();
                await _authStateProvider.NotifyUserLogout();
                _navigationManager.NavigateTo("/", true);
            }

            return response;
        }
}
