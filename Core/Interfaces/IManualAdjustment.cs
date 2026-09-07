namespace Reconciliation.Blazor;

public interface IManualAdjustment
{

    Task<ManualAdjustmentDTO> GetManualAdjustmentRecords(DateTime FromDate, DateTime ToDate, int batchId, int paymentId,int pageNumber,int pageSize,
    string searchValue = null);
    Task<ManualReconciliationResponse> ManualReconcileAsync(ManualReconciliationRequest requestD);
}
