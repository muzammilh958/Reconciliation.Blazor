namespace Reconciliation.Blazor;

public class MerchantCreate
{
    public int batchId { get; set; }
    public int paymentId { get; set; }
    public string? FilePath { get; set; }
    public string? OrderId { get; set; }
    public string? TransactionDate { get; set; }
    public string? Tender { get; set; }
    public string? TransactionAmount { get; set; }
    public string? AuthorizationCode { get; set; }
    public string? MerchantID { get; set; }

    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
    public bool flgActive { get; set; }
    public bool flgDelete { get; set; }
}
