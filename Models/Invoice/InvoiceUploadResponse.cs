using System.Text.Json.Serialization;
using Reconciliation.Blazor.Models.Batch;
namespace Reconciliation.Blazor;

public class InvoiceUploadResponse
{
    public bool success { get; set; }
    public string? message { get; set; }
    public InvoiceUploadedData data { get; set; } = new();
    public int statusCode { get; set; }
    public object? errors { get; set; }
}

public class InvoiceUploadedData
{
    public int batchId { get; set; }
    public int paymentId { get; set; }
    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
    public bool flgActive { get; set; }
    public bool flgDelete { get; set; }
    public string uploadedFilePath { get; set; }
    public string importStatus { get; set; }
    public int errorCount { get; set; }
    public int rowCount { get; set; }
    public object batchName { get; set; }
    public object invoiceType { get; set; }
    public int id { get; set; }
    public object createdAt { get; set; }
    public object updatedAt { get; set; }
    // [JsonPropertyName("batch")]
    // public BatchDataDTO batch { get; set; }
    // [JsonPropertyName("payment")]
    // public PaymentData payment { get; set; }
}
