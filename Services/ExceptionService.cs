using Reconciliation.Blazor.Services;
using Reconciliation.Blazor.Core.Endpoints;
using System.Net.Http.Json;
namespace Reconciliation.Blazor;

public class ExceptionService : IExceptionService
{
    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public ExceptionService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }
    public async Task<ExceptionlistDTO> GetExceptionlist(string batchId)
    {
        try
        {
           var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.ReconciliationAPI.GetException+batchId);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ExceptionlistDTO>();
            
            return result ?? new ExceptionlistDTO() ;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"GetAllAsync error: {ex.Message}");
            throw ex;
        }

    }
}
