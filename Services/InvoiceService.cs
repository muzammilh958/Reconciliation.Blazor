using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
namespace Reconciliation.Blazor;

public class InvoiceService : IInvoiceService
{
    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public InvoiceService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }

    public async Task<InvoiceUploadResponse> CreateAsync(InvoiceUploadRequest model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));


        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Invoice.Create);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(model.batchId.ToString()), "batchId");
            form.Add(new StringContent(model.PaymentId.ToString()), "PaymentId");
            form.Add(new StringContent(model.fromDate.ToString("yyyy-MM-dd")), "fromDate");
            form.Add(new StringContent(model.toDate.ToString("yyyy-MM-dd")), "toDate");


            request.Content = form;

            var response = await _http.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // Try to parse the error response
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<InvoiceUploadResponse>(responseContent);
                    return errorResponse ?? new InvoiceUploadResponse
                    {
                        success = false,
                        message = $"Server error: {response.StatusCode}"
                    };
                }
                catch
                {
                    // If parsing fails, return raw error
                    return new InvoiceUploadResponse
                    {
                        success = false,
                        message = $"Server error: {response.StatusCode} - {responseContent}"
                    };
                }
            }

            var result = await response.Content.ReadFromJsonAsync<InvoiceUploadResponse>();


            return result ?? new InvoiceUploadResponse
            {
                success = false,
                message = "No response from server"
            };
        }
        catch (Exception ex)
        {
            return new InvoiceUploadResponse
            {
                success = false,
                message = ex.Message
            };
        }
    }

}
