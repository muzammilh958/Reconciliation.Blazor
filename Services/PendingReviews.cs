using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
using System.Text.Json;
namespace Reconciliation.Blazor;

public class PendingReviews : IPendingReviewsServices
{
    
    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public PendingReviews(HttpClient http, ITokenProvider tokenProvider)
    {
         _http = http;
        _tokenProvider = tokenProvider;
    }

    public async Task<PendingReviewsDTO> GetPendingReviewsDTOAsync(PendingReviewRequest pendingReviewRequest)
    {
        try
        {
            Console.WriteLine("Hello");
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.ReconciliationAPI.GetManualReconciliationApprovalList);

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(pendingReviewRequest);

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
                        return new PendingReviewsDTO
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
                return new PendingReviewsDTO
                {
                    success = false,
                    message = $"Request failed: {response.StatusCode}",
                    statusCode = (int)response.StatusCode
                };
            }

            var raw = await response.Content.ReadAsStringAsync();

            var dto = JsonSerializer.Deserialize<PendingReviewsDTO>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dto ?? new PendingReviewsDTO();



        }
        catch (Exception ex)
        {
            return new PendingReviewsDTO
            {
                success = false,
                message = ex.Message
            };
        }
    }
    
    public async Task<UpdatePendingReviewsDTO> SetPendingReviewsDTOAsync(ManualReconciliationApprovalRequests pendingReviewRequest)
    {
        try
        {
            
            Console.WriteLine("Hello");
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.ReconciliationAPI.SetManualReconciliationApprovalList);

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(pendingReviewRequest);

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
                        return new UpdatePendingReviewsDTO
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
                return new UpdatePendingReviewsDTO
                {
                    success = false,
                    message = $"Request failed: {response.StatusCode}",
                    statusCode = (int)response.StatusCode
                };
            }

            var raw = await response.Content.ReadAsStringAsync();

            var dto = JsonSerializer.Deserialize<UpdatePendingReviewsDTO>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dto ?? new UpdatePendingReviewsDTO();


        }
        catch(Exception ex)
        { 
            
             return new UpdatePendingReviewsDTO
            {
                success = false,
                message = ex.Message
            };
            
        }     
    }

}
