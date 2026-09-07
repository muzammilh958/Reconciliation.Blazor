namespace Reconciliation.Blazor;

public interface IPendingReviewsServices
{
    Task<PendingReviewsDTO> GetPendingReviewsDTOAsync(PendingReviewRequest pendingReviewRequest);
    Task<UpdatePendingReviewsDTO> SetPendingReviewsDTOAsync(ManualReconciliationApprovalRequests pendingReviewRequest);

}
