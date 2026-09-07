using System.Net.Http.Json;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
using static Reconciliation.Blazor.Core.Endpoints.ApiEndpoints;
using Exception = System.Exception;

namespace Reconciliation.Blazor;

public class StoreService : IStoreService
{
    private readonly HttpClient _http;

    private readonly ITokenProvider _tokenProvider;

    public StoreService(HttpClient http, ITokenProvider tokenProvider)
    {
        _http = http;

        _tokenProvider = tokenProvider;

    }
    public async Task<StoreCreateResponse> CreateAsync(StoreDTO model)
    {
        try
        {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it


            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.Store.Create);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new StoreCreateResponse
                {
                    success = false,
                    message = "Failed to create Store"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<StoreCreateResponse>();

            return result ?? new StoreCreateResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new StoreCreateResponse
            {
                success = false,
                message = ex.Message
            };
        }
    }

    public async Task<StoreDeleteResponse> DeleteAsync(string id)
    {
        try
        {
             var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it

            
            var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoints.Store.Delete + id);
                        request.Headers.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(id);
            var response = await _http.SendAsync(request);
           
            var result = await response.Content.ReadFromJsonAsync<StoreDeleteResponse>();

            return result ?? new StoreDeleteResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {
            return new StoreDeleteResponse
            {
                success = false,
                message  = "Unable to Delete"
            };
        }
    }

    public async Task<List<StoreDTO>> GetAllAsync()
    {
         var token = await _tokenProvider.GetAccessTokenAsync(); 
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Store.GetAll);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API failed: {response.StatusCode} - {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<StoreDTO>>>();

        return result?.Data ?? new List<StoreDTO>();
    }

    public async Task<StoreEditDTO?> GetByIdAsync(int id)
    {
       try
       {
            var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it

            
            var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoints.Store.GetById + id);
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new StoreEditDTO
                {
                    success = false,
                    message = "Failed to fetch Store"
                };
            }
            Console.WriteLine($"Response status code: {response.Content.ReadAsStringAsync()}");
            var result = await response.Content.ReadFromJsonAsync<StoreEditDTO>();
            Console.WriteLine($"Fetched Store: {result?.data?.name}");
            return result;
       }
       catch (Exception e)
       {
            return new StoreEditDTO
            {
                success = false,
                message = $"Unable to fetch Store {e.Message}"
            };
       }
    }

    public async Task<StoreUpdateResponse> UpdateAsync(int id, StoreDTO model)
    {
         try
        {
             var token = await _tokenProvider.GetAccessTokenAsync(); // wherever you store it

            
            var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoints.Store.Update+id);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new StoreUpdateResponse
                {
                    success = false,
                    message = "Failed to Upate Store"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<StoreUpdateResponse>();

            return result ?? new StoreUpdateResponse
            {
                success = false,
                message = "Empty response from server"
            };
        }
        catch (Exception ex)
        {   
            return new StoreUpdateResponse
            {
                success = false,
                message  = "Unable to Update"
            };
        }
    }
}
