using System.Text.Json;
using System.Text.Json.Serialization;
namespace Reconciliation.Blazor;

public class TransactionLineResponse
{
    public bool success { get; set; }
    public string? message { get; set; }
    public List<TransactionLine> data { get; set; } = new();
    public int statusCode { get; set; }
    public object? errors { get; set; }
}   


public class TransactionLine
{
    public int id { get; set; }
    public int batchId { get; set; }
    public int importId { get; set; }
    public string sourceType { get; set; } = "";
    public string merchantSource { get; set; } = "";
    public string orderId { get; set; } = "";
    public string storeId { get; set; } = "";
    public decimal amount { get; set; }
    public string tenderType { get; set; } = "";
    public string authCode { get; set; } = "";
    public DateTime transactionDate { get; set; }
    public DateTime businessDay { get; set; }
    


    
    public decimal? varianceAmount { get; set; }
    public string importedBy { get; set; } = "";
    public DateTime importedAt { get; set; }
    public bool isDeleted { get; set; }
    public bool flgDelete { get; set; }
    public bool flgActive { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    
}
