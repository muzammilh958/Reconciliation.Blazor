using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Reconciliation.Blazor.Models.Auth;

namespace Reconciliation.Blazor.Services
{
    /// <summary>
    /// HTTP client that automatically injects JWT bearer tokens into outgoing requests
    /// and handles token refresh on 401 responses.
    /// </summary>
    public class AuthorizedHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AuthorizedHttpClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthorizedHttpClient(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IAuthenticationService authService,
            ILogger<AuthorizedHttpClient> logger)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authService = authService;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Adds the JWT bearer token from storage to the Authorization header.
        /// </summary>
        private async Task AddAuthorizationHeaderAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync("access_token");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", token);
                    _logger.LogDebug("JWT token added to Authorization header");
                }
                else
                {
                    // Clear any existing authorization header if no token
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving token from storage");
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        /// <summary>
        /// Attempts to refresh the JWT token and retry the request.
        /// </summary>
        private async Task<HttpResponseMessage?> HandleUnauthorizedAsync(
            Func<Task<HttpResponseMessage>> requestFunc)
        {
            try
            {
                _logger.LogWarning("Received 401 Unauthorized, attempting token refresh");

                var refreshToken = await _localStorage.GetItemAsync("refresh_token");
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("No refresh token available, cannot refresh access token");
                    return null;
                }

                // Attempt to refresh the token
                var refreshRequest = new RefreshTokenRequest { RefreshToken = refreshToken };
                var refreshResponse = await _authService.RefreshTokenAsync(refreshRequest);

                if (!refreshResponse.Success)
                {
                    _logger.LogWarning("Token refresh failed: {message}", refreshResponse.Message);
                    await _authService.ClearTokensAsync();
                    return null;
                }

                // Retry the original request with the new token
                _logger.LogInformation("Token refresh successful, retrying original request");
                await AddAuthorizationHeaderAsync();
                return await requestFunc();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh attempt");
                return null;
            }
        }

        /// <summary>
        /// Performs a GET request with automatic token injection and 401 handling.
        /// </summary>
        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var retryResponse = await HandleUnauthorizedAsync(
                    async () =>
                    {
                        await AddAuthorizationHeaderAsync();
                        return await _httpClient.GetAsync(uri);
                    });

                if (retryResponse != null)
                {
                    return retryResponse;
                }
            }

            _logger.LogDebug("GET {Uri} returned {StatusCode}", uri, response.StatusCode);
            return response;
        }

        /// <summary>
        /// Performs a POST request with automatic token injection and 401 handling.
        /// </summary>
        public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsync(uri, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var retryResponse = await HandleUnauthorizedAsync(
                    async () =>
                    {
                        await AddAuthorizationHeaderAsync();
                        return await _httpClient.PostAsync(uri, content);
                    });

                if (retryResponse != null)
                {
                    return retryResponse;
                }
            }

            _logger.LogDebug("POST {Uri} returned {StatusCode}", uri, response.StatusCode);
            return response;
        }

        /// <summary>
        /// Performs a PUT request with automatic token injection and 401 handling.
        /// </summary>
        public async Task<HttpResponseMessage> PutAsync(string uri, HttpContent content)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsync(uri, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var retryResponse = await HandleUnauthorizedAsync(
                    async () =>
                    {
                        await AddAuthorizationHeaderAsync();
                        return await _httpClient.PutAsync(uri, content);
                    });

                if (retryResponse != null)
                {
                    return retryResponse;
                }
            }

            _logger.LogDebug("PUT {Uri} returned {StatusCode}", uri, response.StatusCode);
            return response;
        }

        /// <summary>
        /// Performs a DELETE request with automatic token injection and 401 handling.
        /// </summary>
        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync(uri);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var retryResponse = await HandleUnauthorizedAsync(
                    async () =>
                    {
                        await AddAuthorizationHeaderAsync();
                        return await _httpClient.DeleteAsync(uri);
                    });

                if (retryResponse != null)
                {
                    return retryResponse;
                }
            }

            _logger.LogDebug("DELETE {Uri} returned {StatusCode}", uri, response.StatusCode);
            return response;
        }

        /// <summary>
        /// Performs a GET request and deserializes the response to JSON with token injection.
        /// </summary>
        public async Task<T?> GetJsonAsync<T>(string uri)
        {
            try
            {
                var response = await GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("GET {Uri} returned 401 after retry", uri);
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET {Uri} returned {StatusCode}", uri, response.StatusCode);
                    return default;
                }

                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                _logger.LogDebug("GET {Uri} successfully deserialized to {Type}", uri, typeof(T).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing GET response from {Uri}", uri);
                return default;
            }
        }

        /// <summary>
        /// Performs a POST request with JSON data and deserializes the response with token injection.
        /// </summary>
        public async Task<T?> PostJsonAsync<T>(string uri, object data)
        {
            try
            {
                var content = JsonContent.Create(data);
                var response = await PostAsync(uri, content);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("POST {Uri} returned 401 after retry", uri);
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST {Uri} returned {StatusCode}", uri, response.StatusCode);
                    return default;
                }

                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                _logger.LogDebug("POST {Uri} successfully deserialized to {Type}", uri, typeof(T).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during POST request to {Uri}", uri);
                return default;
            }
        }

        /// <summary>
        /// Performs a PUT request with JSON data and deserializes the response with token injection.
        /// </summary>
        public async Task<T?> PutJsonAsync<T>(string uri, object data)
        {
            try
            {
                var content = JsonContent.Create(data);
                var response = await PutAsync(uri, content);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("PUT {Uri} returned 401 after retry", uri);
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT {Uri} returned {StatusCode}", uri, response.StatusCode);
                    return default;
                }

                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                _logger.LogDebug("PUT {Uri} successfully deserialized to {Type}", uri, typeof(T).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during PUT request to {Uri}", uri);
                return default;
            }
        }

        /// <summary>
        /// Performs a PATCH request with JSON data and deserializes the response with token injection.
        /// </summary>
        public async Task<T?> PatchJsonAsync<T>(string uri, object data)
        {
            try
            {
                var content = JsonContent.Create(data);
                var request = new HttpRequestMessage(HttpMethod.Patch, uri) { Content = content };

                await AddAuthorizationHeaderAsync();
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var retryResponse = await HandleUnauthorizedAsync(
                        async () =>
                        {
                            var retryRequest = new HttpRequestMessage(HttpMethod.Patch, uri) 
                            { 
                                Content = JsonContent.Create(data) 
                            };
                            await AddAuthorizationHeaderAsync();
                            return await _httpClient.SendAsync(retryRequest);
                        });

                    if (retryResponse != null)
                    {
                        response = retryResponse;
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PATCH {Uri} returned {StatusCode}", uri, response.StatusCode);
                    return default;
                }

                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                _logger.LogDebug("PATCH {Uri} successfully deserialized to {Type}", uri, typeof(T).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during PATCH request to {Uri}", uri);
                return default;
            }
        }
    }
}
