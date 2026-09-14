using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
using static Reconciliation.Blazor.Pages.Merchant.MerchantUpload;

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
    public async Task<MerchantCreateResponse> CreateAsync(MerchantCreate model, List<byte[]> fileBytes, List<string> fileNames, List<string> contentTypes)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

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

            // Change "files" to "file" to match server expectation
            for (int i = 0; i < fileBytes.Count; i++)
            {
                var fileContent = new ByteArrayContent(fileBytes[i]);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentTypes[i]);
                form.Add(fileContent, "file", fileNames[i]); // Changed from "files" to "file"
            }

            request.Content = form;


            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                MerchantCreateResponse? errorResponse = null;

                try
                {
                    errorResponse = JsonSerializer.Deserialize<MerchantCreateResponse>(
                        errorContent,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
                catch
                {
                    // Ignore parsing errors
                }

                return new MerchantCreateResponse
                {
                    success = false,
                    message = errorResponse?.message
                              ?? "Unable to process your request. Please try again."
                };
            }

            var result = await response.Content.ReadFromJsonAsync<MerchantCreateResponse>();
            if (result == null)
            {
                return new MerchantCreateResponse
                {
                    success = false,
                    message = "Unable to read server response."
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                result.success = false;

                // In case API didn't populate message
                if (string.IsNullOrWhiteSpace(result.message))
                {
                    result.message = $"Request failed ({(int)response.StatusCode})";
                }
            }
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
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"DownloadMerchantFile error: {ex.Message}");
            return null;
        }
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
