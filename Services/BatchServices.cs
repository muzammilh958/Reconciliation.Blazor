using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Models.Batch;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class BatchServices : IBatchesService
{


    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public BatchServices(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;

        _tokenProvider = tokenProvider;

    }


    public async Task<BatchCreateResponse> CreateAsync(BatchDataDTO model)
    {
        try
        {

            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it
            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Batch.Create);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new BatchCreateResponse
                {
                    success = false,
                    message = "Failed to create batch"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<BatchCreateResponse>();

            return result ?? new BatchCreateResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new BatchCreateResponse
            {
                success = false,
                message = ex.Message
            };
        }

    }

    public async Task<BatchDeleteResponse> DeleteAsync(string id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoints.Batch.Delete + id);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(id);
            var response = await _http.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<BatchDeleteResponse>();

            return result ?? new BatchDeleteResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new BatchDeleteResponse
            {
                success = false,
                message = "Unable to Delete"
            };
        }
    }

    public async Task<List<BatchDataDTO>> GetAllAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Batch.GetAll);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<BatchDataDTO>>>();

            return result?.Data ?? new List<BatchDataDTO>();
        }
        catch (Exception ex)
        {
            return new List<BatchDataDTO>();
        }
    }

    public async Task<BatchDataDTO?> GetByIdAsync(int id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Batch.GetById + id);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<BatchDataDTO>>();

            return result?.Data ?? new BatchDataDTO();
        }
        catch (Exception ex)
        {
            return new BatchDataDTO();
        }
    }

    public async Task<BatchUpdateResponse> UpdateAsync(int id, BatchDataDTO model)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoints.Batch.Update + id);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new BatchUpdateResponse
                {
                    success = false,
                    message = "Failed to Upate batch"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<BatchUpdateResponse>();

            return result ?? new BatchUpdateResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new BatchUpdateResponse
            {
                success = false,
                message = "Unable to Update"
            };
        }

    }


    public async Task<ReconciliationResult> ReconcileBatchAsync(DateTime StartDate, DateTime EndDate, int batchId, int paymentId)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.ReconciliationAPI.StartReconciliation);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            // request.Content = JsonContent.Create(id);

            request.Content = JsonContent.Create(new
            {
                StartDate,
                EndDate,
                batchId,
                paymentId
            });


            var response = await _http.SendAsync(request);
            var contentString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(contentString);
            var result = await response.Content.ReadFromJsonAsync<ReconciliationResult>();

            return result ?? new ReconciliationResult
            {
                success = false,
                statusCode = (int)response.StatusCode,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new ReconciliationResult
            {
                success = false,
                statusCode = 500,
                message = ex.Message
            };
        }
    }

}
