using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    public async Task<InvoiceUploadResponse> CreateAsync(InvoiceUploadRequest model, byte[] fileBytes, string fileName, string contentType)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (fileBytes == null || fileBytes.Length == 0)
            throw new ArgumentException("File bytes cannot be null or empty.", nameof(fileBytes));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Invoice.Create);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(model.batchId.ToString()), "batchId");
            form.Add(new StringContent(model.invoiceId.ToString()), "invoiceId");
            form.Add(new StringContent(model.fromDate.ToString("yyyy-MM-dd")), "fromDate");
            form.Add(new StringContent(model.toDate.ToString("yyyy-MM-dd")), "toDate");

            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            form.Add(fileContent, "File", fileName);

            request.Content = form;

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new InvoiceUploadResponse
                {
                    success = false,
                    message = $"Server error: {response.StatusCode} - {errorContent}"
                };
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
