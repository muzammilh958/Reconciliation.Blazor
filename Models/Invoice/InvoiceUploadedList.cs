namespace Reconciliation.Blazor;

public class InvoiceUploadedList
{
    public bool success { get; set; }
    public string message { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
    public List<InvoiceUploaded> data { get; set; }

}

public class InvoiceUploaded
{
    public int batchId { get; set; }
    public int invoiceId { get; set; }
    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
    public bool flgActive { get; set; }
    public bool flgDelete { get; set; }
    public string uploadedFilePath { get; set; }
    public string importStatus { get; set; }
    public int errorCount { get; set; }
    public int rowCount { get; set; }
    public int id { get; set; }
    public string batchName { get; set; }
    public string invoiceType { get; set; }
    public DateTime? createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
}
