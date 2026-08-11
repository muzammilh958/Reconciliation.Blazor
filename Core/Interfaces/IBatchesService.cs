using Reconciliation.Blazor.Models.Batch;

namespace Reconciliation.Blazor;

public interface IBatchesService
{
    Task<List<BatchDataDTO>> GetAllAsync();
    Task<BatchDataDTO?> GetByIdAsync(int id);

    Task<BatchCreateResponse> CreateAsync(BatchDataDTO model);

    Task<BatchUpdateResponse> UpdateAsync(int id, BatchDataDTO request);

    Task<BatchDeleteResponse> DeleteAsync(string id);
    Task<ReconciliationResult> ReconcileBatchAsync(DateTime FromDate, DateTime ToDate ,int batchId, int paymentId);
}
