using System.Net.Http.Headers;
using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
namespace Reconciliation.Blazor;

public class InvoiceTypeService : IInvoiceTypeService
{
    private readonly HttpClient _http;
    private readonly ITokenProvider _tokenProvider;
    public InvoiceTypeService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;
        _tokenProvider = tokenProvider;
    }
    public async Task<InvoiceCreateResponse> CreateAsync(InvoiceType model)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it

            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.InvoiceType.Create);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new InvoiceCreateResponse
                {
                    success = false,
                    message = "Failed to create InvoiceType"
                };
            }
            var responseBody = await response.Content.ReadFromJsonAsync<InvoiceCreateResponse>();
            return responseBody ?? new InvoiceCreateResponse
            {
                success = false,
                message = "Failed to create InvoiceType"
            };
        }
        catch (Exception e)
        {
            return new InvoiceCreateResponse
            {
                success = false,
                message = e.Message
            };
        }

    }

    public async Task<InvoiceDeleteResponse> DeleteAsync(string id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiEndpoints.InvoiceType.Delete}/{id}");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new InvoiceDeleteResponse
                {
                    success = false,
                    message = "Failed to delete InvoiceType"
                };
            }
            var responseBody = await response.Content.ReadFromJsonAsync<InvoiceDeleteResponse>();
            return responseBody ?? new InvoiceDeleteResponse
            {
                success = false,
                message = "Failed to delete InvoiceType"
            };
        }
        catch (Exception e)
        {
             return new InvoiceDeleteResponse
            {
                success = false,
                message = e.Message
            };
        }
    }

    public async Task<InvoiceTypeList> GetAllAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.InvoiceType.GetAll);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to retrieve InvoiceTypes");
            }
            var responseBody = await response.Content.ReadFromJsonAsync<InvoiceTypeList>();
            return responseBody ?? new InvoiceTypeList();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }

    public async Task<SingleInvoiceTypeResponse?> GetByIdAsync(int id)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoints.InvoiceType.GetById}{id}");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            Console.WriteLine($"Response: {response.StatusCode}");
            Console.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to retrieve InvoiceType");
            }
            var responseBody = await response.Content.ReadFromJsonAsync<SingleInvoiceTypeResponse>();
            Console.WriteLine($"Response Body: {responseBody.data.id}, {responseBody.data.name}");

            return responseBody;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }

    public async Task<InvoiceUpdateResponse> UpdateAsync(int id, InvoiceType request)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"{ApiEndpoints.InvoiceType.Update}{id}");
            httpRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            httpRequest.Content = JsonContent.Create(request);
            var response = await _http.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                return new InvoiceUpdateResponse
                {
                    success = false,
                    message = "Failed to update InvoiceType"
                };
            }
            var responseBody = await response.Content.ReadFromJsonAsync<InvoiceUpdateResponse>();
            return responseBody ?? new InvoiceUpdateResponse
            {
                success = false,
                message = "Failed to update InvoiceType"
            };
        }
        catch (Exception e)
        {
            return new InvoiceUpdateResponse
            {
                success = false,
                message = e.Message
            };
        }
    }
    
    public async Task<InvoiceUploadedList> GetUploadedInvoicesAsync()
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Invoice.GetAll);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to retrieve uploaded invoices");
            }
            var responseBody = await response.Content.ReadFromJsonAsync<InvoiceUploadedList>();
            return responseBody ?? new InvoiceUploadedList();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }

    public async Task<byte[]?> DownloadInvoiceFile(int id)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiEndpoints.Invoice.Download}/{id}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsByteArrayAsync();
    }

   
}
