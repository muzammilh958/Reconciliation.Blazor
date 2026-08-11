using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace Reconciliation.Blazor;

 public class ApiAuthHandler : DelegatingHandler
    {
        private readonly TokenServices _tokenServices;
        private readonly NavigationManager _navigationManager;

        public ApiAuthHandler(TokenServices tokenServices, NavigationManager navigationManager)
        {
            _tokenServices = tokenServices;
            _navigationManager = navigationManager;
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var token = await _tokenServices.GetToken();
                
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Clear invalid token
                    await _tokenServices.RemoveToken();
                    
                    // Redirect to login
                    _navigationManager.NavigateTo("/", true);
                }

                return response;
            }
            catch
            {
                // If any error, redirect to login
                _navigationManager.NavigateTo("/", true);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }