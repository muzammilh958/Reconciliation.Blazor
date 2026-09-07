using Reconciliation.Blazor.Models.Batch;

namespace Reconciliation.Blazor;

public interface IBatchesService
{
    Task<List<BatchDataDTO>> GetAllAsync();
    Task<BatchDataDTO?> GetByIdAsync(int id);

    Task<BatchCreateResponse> CreateAsync(BatchDataDTO model);

    Task<BatchUpdateResponse> UpdateAsync(int id, BatchDataDTO request);

    Task<BatchDeleteResponse> DeleteAsync(string id);
    Task<ReconciliationResultsDTO> ReconcileBatchAsync(DateTime FromDate, DateTime ToDate, int batchId, int paymentId);

    Task<bool> LockBatch(int id);
    Task<bool> LockBatchReco(int id,bool locked);

    Task<DeleteReconciliationResponse> VoidBatch(int RecoId);

    Task<bool> StartReconcileBatchAsync(DateTime fromDate, DateTime toDate, int batchId, int paymentId);
    Task<ReconciliationResultsDTO?> GetReconciliationResultAsync(int batchId, int paymentId);

    Task<byte[]?> DownloadReconciliationFile(int id);

    Task LockBatch(int id, bool locked);
}
