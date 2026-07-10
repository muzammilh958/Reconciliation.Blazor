
using static Reconciliation.Blazor.Core.Endpoints.ApiEndpoints;
namespace Reconciliation.Blazor;

public interface IStoreService
{
    Task<List<StoreDTO>> GetAllAsync();
    Task<StoreDTO?> GetByIdAsync(int id);

    Task<StoreCreateResponse> CreateAsync(StoreDTO model);

    Task<StoreUpdateResponse> UpdateAsync(int id, StoreDTO request);

    Task<StoreDeleteResponse> DeleteAsync(string id);
}
