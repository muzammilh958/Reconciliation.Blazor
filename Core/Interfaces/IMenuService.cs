namespace Reconciliation.Blazor;

public interface IMenuService
{

    Task<List<MenuItemDto>> GetAllAsync();
    // Task<Batch?> GetByIdAsync(int id);

    // Task<BatchCreateResponse> CreateAsync(Batch model);

    // Task<BatchUpdateResponse> UpdateAsync(int id, Batch request);

    // Task<BatchDeleteResponse> DeleteAsync(string id);
}
