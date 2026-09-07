using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
namespace Reconciliation.Blazor;

public class ExceptionsServices : IExceptions
{
    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public ExceptionsServices(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }
    public async Task<RecReconciliationExceptionDTO> GetExceptionsAsync(int batchId, int pageNumber, int pageSize, string searchValue = null, string sortColumn = null, string sortDirection = "desc")
    {
        try
        {
           
             var query = $"?pageNumber={pageNumber}&pageSize={pageSize}" +
                $"&searchValue={Uri.EscapeDataString(searchValue ?? "")}" +
                $"&sortColumn={sortColumn}&sortDirection={sortDirection}";


            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoints.Exception.getAllException}{batchId}{query}");

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
           
            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    // Try to deserialize the error response
                    var errorDto = JsonSerializer.Deserialize<PendingReviewsDTO>(errorContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (errorDto != null && !string.IsNullOrEmpty(errorDto.message))
                    {
                        // Return the DTO with the clean message
                        return new RecReconciliationExceptionDTO
                        {
                            success = false,
                            message = errorDto.message,
                            statusCode = (int)response.StatusCode
                        };
                    }
                }
                catch
                {
                    // If deserialization fails, fall back to a generic message
                }

                // Fallback: return a generic error
                return new RecReconciliationExceptionDTO
                {
                    success = false,
                    message = $"Request failed: {response.StatusCode}",
                    statusCode = (int)response.StatusCode
                };
            }

            var raw = await response.Content.ReadAsStringAsync();
            
            var dto = JsonSerializer.Deserialize<RecReconciliationExceptionDTO>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
          
            return dto ?? new RecReconciliationExceptionDTO();



        }
        catch (Exception ex)
        {
            return new RecReconciliationExceptionDTO
            {
                success = false,
                message = ex.Message
            };
        }

    }
}
