using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Models.Batch;
using Reconciliation.Blazor.Services;
using Microsoft.AspNetCore.Components;


namespace Reconciliation.Blazor;

public class BatchServices : IBatchesService
{

   
    private readonly HttpClient _http;
    private readonly ApiAuthHandler _apiAuthHandler;
    public BatchServices(HttpClient http, NavigationManager navigation, ApiAuthHandler apiAuthHandler)
    {
        _http = http;
        _apiAuthHandler = apiAuthHandler;
    }


    public async Task<BatchCreateResponse> CreateAsync(BatchDataDTO model)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.Batch.Create, model);
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

            
            var response = await _http.DeleteAsync(ApiEndpoints.Batch.Delete + id);

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
            var response = await _http.GetAsync(ApiEndpoints.Batch.GetAll);

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
            var response = await _http.GetAsync(ApiEndpoints.Batch.GetById + id);

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
            var response = await _http.PutAsJsonAsync(ApiEndpoints.Batch.Update + id, model);

            if (!response.IsSuccessStatusCode)
            {
                return new BatchUpdateResponse
                {
                    success = false,
                    message = "Failed to Upate batch"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<BatchUpdateResponse>();

            return result ?? new BatchUpdateResponse { success = false, message = "Empty response from server" };
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


    public async Task<ReconciliationResultsDTO> ReconcileBatchAsync(DateTime StartDate, DateTime EndDate, int batchId, int paymentId)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.ReconciliationAPI.StartReconciliation, new
            { StartDate, EndDate, batchId, paymentId });

            var result = await response.Content.ReadFromJsonAsync<ReconciliationResultsDTO>();

            return result ?? new ReconciliationResultsDTO
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                Message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new ReconciliationResultsDTO
            {
                Success = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }


    public async Task<bool> LockBatch(int id)
    {

        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.ReconciliationAPI.batchLocked + id, new { id });
            var contentString = await response.Content.ReadAsStringAsync();
            
            var result = await response.Content.ReadFromJsonAsync<ReconciliationResultsDTO>();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return false;
        }
    }


    public async Task<DeleteReconciliationResponse> VoidBatch(int RecoId)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
               ApiEndpoints.ReconciliationAPI.DeleteReconciliationResult,
               new { RecoId });
                
            var contentString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DeleteReconciliationResponse>();
                return result ?? new DeleteReconciliationResponse
                {
                    Success = false,
                    Message = "Empty response from server"
                };
            }
            else
            {
                // Try to parse error response
                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<DeleteReconciliationResponse>();
                    if (errorResult != null && !string.IsNullOrEmpty(errorResult.Message))
                    {
                        return new DeleteReconciliationResponse
                        {
                            Success = false,
                            Message = errorResult.Message
                        };
                    }
                }
                catch { /* Ignore deserialization errors */ }

                return new DeleteReconciliationResponse
                {
                    Success = false,
                    Message = $"API Error ({(int)response.StatusCode}): {response.ReasonPhrase}"
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new DeleteReconciliationResponse
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<bool> StartReconcileBatchAsync(DateTime fromDate, DateTime toDate, int batchId, int paymentId)
    {
        var response = await _http.PostAsJsonAsync("api/Reconciliation/start", new
        {
            StartDate = fromDate,
            EndDate = toDate,
            BatchId = batchId,
            PaymentId = paymentId
        });
        return response.StatusCode == System.Net.HttpStatusCode.Accepted;
    }

    public async Task<bool> LockBatchReco(int batchId,bool locked)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
                ApiEndpoints.ReconciliationAPI.LockBatchReco + batchId,
                new { locked });

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LockBatch Error: {ex.Message}");
            return false;
        }
    }

    public async Task<ReconciliationResultsDTO?> GetReconciliationResultAsync(int batchId, int paymentId)
    {
         return await _http.GetFromJsonAsync<ReconciliationResultsDTO>(
            $"api/Reconciliation/result/{batchId}/{paymentId}");
    }

    public async Task<byte[]?> DownloadReconciliationFile(int id)
    {
        var response = await _http.GetAsync(
            $"{ApiEndpoints.ReconciliationAPI.DownloadReconciliationResult}{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task LockBatch(int id, bool locked)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
                ApiEndpoints.ReconciliationAPI.LockBatch + id,
                new { locked });

            _ = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }


}
