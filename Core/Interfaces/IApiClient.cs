namespace Reconciliation.Blazor.Core.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest request);
        Task<TResponse?> GetAsync<TResponse>(string url);
    }
}