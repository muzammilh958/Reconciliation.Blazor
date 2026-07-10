using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class GlobalSettingService : IGlobalSettings
{

    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public GlobalSettingService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }


    public async Task<List<GlobalSetting>> GetAllAsync()
    {
        try
        {
           
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Settings.GetAll);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<GlobalSetting>>>();
            
            return result?.Data ?? new List<GlobalSetting>() ;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"GetAllAsync error: {ex.Message}");
            throw ex;
        }

    }

    public Task<GlobalSetting> UpdateAsync(int id, GlobalSetting request)
    {
        throw new NotImplementedException();
    }
}
