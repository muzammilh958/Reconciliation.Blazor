using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class PaymentService : IPaymentService
{

    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public PaymentService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }

    public async Task<PaymentCreateResponse> CreateAsync(PaymentData model)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it
            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Payment.Create);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new PaymentCreateResponse
                {
                    success = false,
                    message = "Failed to create Payment"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<PaymentCreateResponse>();

            return result ?? new PaymentCreateResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new PaymentCreateResponse
            {
                success = false,
                message = ex.Message
            };
        }
    }

    public async Task<PaymentDeleteResponse> DeleteAsync(string id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoints.Payment.Delete + id);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(id);
            var response = await _http.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<PaymentDeleteResponse>();

            return result ?? new PaymentDeleteResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new PaymentDeleteResponse
            {
                success = false,
                message = "Unable to Delete"
            };
        }
    }

    public async Task<List<PaymentData>> GetAllAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Payment.GetAll);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<PaymentData>>>();

            return result?.Data ?? new List<PaymentData>();
        }
        catch (Exception ex)
        {
            return new List<PaymentData>();
        }
    }

    public Task<PaymentData?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentUpdateResponse> UpdateAsync(int id, PaymentData model)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoints.Payment.Update + id);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new PaymentUpdateResponse
                {
                    success = false,
                    message = "Failed to Upate Payment"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<PaymentUpdateResponse>();

            return result ?? new PaymentUpdateResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {

            
            return new PaymentUpdateResponse
            {
                success = false,
                message = "Unable to Update"+ex.Message.ToString()
            };
        }

    }
}
