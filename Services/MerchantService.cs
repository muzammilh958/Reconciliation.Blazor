using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;

namespace Reconciliation.Blazor;

public class MerchantService : IMerchantService
{

    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public MerchantService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }
    public async Task<MerchantCreateResponse> CreateAsync(MerchantCreate model, byte[] fileBytes, string fileName, string contentType)
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

            using var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Merchant.Create);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(model.batchId.ToString()), "batchId");
            form.Add(new StringContent(model.paymentId.ToString()), "paymentId");
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
                return new MerchantCreateResponse
                {
                    success = false,
                    message = $"Server error: {response.StatusCode} - {errorContent}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<MerchantCreateResponse>();

            return result ?? new MerchantCreateResponse
            {
                success = false,
                message = "No response from server"
            };
        }
        catch (Exception ex)
        {
            return new MerchantCreateResponse
            {
                success = false,
                message = ex.Message
            };
        }
    }

    public async Task<MerchantUploadResponse> GetAllMerchantUploadData()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Merchant.MerchatUploadList);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<MerchantUploadResponse>();
            // Console.WriteLine("Merchant Service Hello======");

            // Console.WriteLine(result.data.Count);
            return result ?? new MerchantUploadResponse
            {
                success = false,
                message = "Empty response",
                data = new List<UploadedData>()
            };


        }
        catch (Exception ex)
        {
            return new MerchantUploadResponse
            {
                success = false,
                message = ex.Message,
                data = new List<UploadedData>()
            };
        }
    }
    
    public async Task<byte[]?> DownloadMerchantFile(int id)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiEndpoints.Merchant.Download}/{id}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<TransactionLineResponse> GetAllTransactionLines(string id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoints.Merchant.TransactionLines}{id}");

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API failed: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<TransactionLineResponse>();

            return result ?? new TransactionLineResponse
            {
                success = false,
                message = "Empty response",
                data = new List<TransactionLine>()
            };
        }
        catch (Exception ex)
        {
            return new TransactionLineResponse
            {
                success = false,
                message = ex.Message,
                data = new List<TransactionLine>()
            };
        }
    }
}
