using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
namespace Reconciliation.Blazor;

public class DashboardService : IDashboardService
{ 
    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public DashboardService(HttpClient http, ITokenProvider tokenProvider)
    {
         _http = http;
        _tokenProvider = tokenProvider;
    }

    public async Task<DashboardResponse> GetAllAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it

            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Dashboard.GetBatchCount);
            
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await _http.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<DashboardResponse>();

            return result ?? new DashboardResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new DashboardResponse
            {
                success = false,
                message = "Unable to Fetch Data"
            };
        }
    }
}
