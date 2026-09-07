using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class ManualAdjustmentService : IManualAdjustment
{

    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public ManualAdjustmentService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }
    public async Task<ManualAdjustmentDTO> GetManualAdjustmentRecords(DateTime StartDate, DateTime EndDate, int batchId, int paymentId,int pageNumber,int pageSize,
    string searchValue = null)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.ReconciliationAPI.GetManualReconciliationResults+"?pageNumber="+pageNumber+"&pageSize="+pageSize+$"&searchValue={searchValue ?? ""}");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            // request.Content = JsonContent.Create(id);

            request.Content = JsonContent.Create(new
            {
                StartDate,
                EndDate,
                batchId,
                paymentId,
                pageNumber,
                pageSize
            });


            var response = await _http.SendAsync(request);
            var contentString = await response.Content.ReadAsStringAsync();
          
            var result = await response.Content.ReadFromJsonAsync<ManualAdjustmentDTO>();

            return result ?? new ManualAdjustmentDTO
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                Message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new ManualAdjustmentDTO
            {
                Success = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }


    public async Task<ManualReconciliationResponse> ManualReconcileAsync(ManualReconciliationRequest request)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                ApiEndpoints.ReconciliationAPI.SetManualTransactionForReconciliation);

            httpRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token);

            httpRequest.Content = JsonContent.Create(request);

            var response = await _http.SendAsync(httpRequest);

            var result = await response.Content
                .ReadFromJsonAsync<ManualReconciliationResponse>();

            if (result == null)
            {
                throw new Exception("No response received from server.");
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new ManualReconciliationResponse
            {
                // Success = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }
}
