namespace Reconciliation.Blazor;

public class InvoiceUploadRequest
{
    public int batchId { get; set; }
    public int invoiceId { get; set; }
    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
}
