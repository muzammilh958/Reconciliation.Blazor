using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Serialization;
using Reconciliation.Blazor.Models.Batch;
namespace Reconciliation.Blazor;

public class MerchantUploadResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<UploadedData> data { get; set; } = new();
    public int statusCode { get; set; }
    public object? errors { get; set; }
}
public class UploadedData
{
    public int id { get; set; }
    public int batchId { get; set; }
    public int paymentId { get; set; }
    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
    public bool flgActive { get; set; }
    public bool flgDelete { get; set; }
    public string uploadedFilePath { get; set; } = "";

    public int rowCount { get; set; }
    public int errorCount { get; set; }
    public string importStatus { get; set; } = "";
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    [JsonPropertyName("batch")]
    public BatchDataDTO batch { get; set; }
    [JsonPropertyName("payment")]
    public PaymentData payment { get; set; }
}
