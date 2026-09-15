namespace Reconciliation.Blazor;


public class PendingReviewRequest
{
    public int PaymentId { get; set; }
    public int BatchId { get; set; }
}
public class PendingReviewsDTO
{
    public bool success { get; set; } = false;
    public string message { get; set; } ="";
    public List<PendingReviewsData> data { get; set; }  = new();
    public int statusCode { get; set; }
    public object errors { get; set; }
}


public class PendingReviewsData
{
    public int id { get; set; }
    public int reconciliationId { get; set; }
    public int matchStep { get; set; }
    public int invoiceTransactionId { get; set; }
    public int merchantTransactionId { get; set; }
    public bool flgDelete { get; set; }
    public bool flgActive { get; set; }
    public bool flgReview { get; set; }
    public DateTime createdAt { get; set; }
    public object updatedAt { get; set; }
    public object reconciliation { get; set; }
    public InvoiceTransaction invoiceTransaction { get; set; }
    public MerchantTransaction merchantTransaction { get; set; }
}

public class InvoiceTransaction
{
    public int batchId { get; set; }
    public int importId { get; set; }
    public string sourceType { get; set; }
    public string invoiceSource { get; set; }
    public object orderId { get; set; }
    public string storeId { get; set; }
    public double amount { get; set; }
    public string tenderType { get; set; }
    public string authCode { get; set; }
    public DateTime transactionDate { get; set; }
    public DateTime businessDay { get; set; }
    public object varianceAmount { get; set; }
    public bool flgActive { get; set; }
    public bool flgDelete { get; set; }
    public bool flgDuplicate { get; set; }
    public bool flgReco { get; set; }
    public string importedBy { get; set; }
    public DateTime importedAt { get; set; }
    public int id { get; set; }
    public DateTime createdAt { get; set; }
    public object updatedAt { get; set; }
}

public class MerchantTransaction
{
    public int batchId { get; set; }
    public int importId { get; set; }
    public string sourceType { get; set; }
    public string merchantSource { get; set; }
    public object orderId { get; set; }
    public string storeId { get; set; }
    public double amount { get; set; }
    public string tenderType { get; set; }
    public string authCode { get; set; }
    public DateTime transactionDate { get; set; }
    public DateTime businessDay { get; set; }
    public object varianceAmount { get; set; }
    public string importedBy { get; set; }
    public DateTime importedAt { get; set; }
    public bool flgDelete { get; set; }
    public bool flgActive { get; set; }
    public bool flgDuplicate { get; set; }
    public bool flgReco { get; set; }
    public int id { get; set; }
    public DateTime createdAt { get; set; }
    public object updatedAt { get; set; }
}

public class ManualReconciliationApprovalRequests
{
    public int Id { get; set; }
    public bool ApprovalStatus { get; set; }
}



public class UpdatePendingReviewsDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public PendingReviewsData data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}