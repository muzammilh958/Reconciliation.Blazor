namespace Reconciliation.Blazor;

// public class ReconciliationResult
// {
//     public bool success { get; set; }
//     public string message { get; set; }
//     public List<TransactionLine1> data { get; set; } = new();
//     public int statusCode { get; set; }
//     public object errors { get; set; }
// }



public class Batch
{
    public int id { get; set; }
    public string? name { get; set; }
    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
    public bool isDeleted { get; set; }
    public bool is_locked { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}
public class Payment
{
    public string? name { get; set; }
    public string? orderId { get; set; }
    public string? transactionDate { get; set; }
    public string? tender { get; set; }
    public string? transactionAmount { get; set; }
    public string? authorizationCode { get; set; }
    public string merchantID { get; set; }
    public bool? isAuthEnable { get; set; }
    public bool? flgActive { get; set; }
    public bool? flgDelete { get; set; }
    public int? id { get; set; }
    public DateTime? createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
}

public class Datum
{
    public int id { get; set; }
    public int batchId { get; set; }
    public Batch? batch { get; set; }
    public int? invoiceLineId { get; set; }
    public InvoiceLine? invoiceLine { get; set; }
    public int? merchantLineId { get; set; }
    public MerchantLine? merchantLine { get; set; }
    public int? matchStep { get; set; }
    public string? reconStatus { get; set; }
    public string? exceptionType { get; set; }
    public double? varianceAmount { get; set; }
    public DateTime? matchedAt { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
}

public class InvoiceLine
{
    public int batchId { get; set; }
    public int importId { get; set; }
    public string? sourceType { get; set; }
    public string? invoiceSource { get; set; }
    public string? orderId { get; set; }
    public string? storeId { get; set; }
    public double amount { get; set; }
    public string? tenderType { get; set; }
    public string? authCode { get; set; }
    public DateTime transactionDate { get; set; }
    public DateTime businessDay { get; set; }
    public double? varianceAmount { get; set; }
    public bool flgActive { get; set; }
    public bool flgDelete { get; set; }
    public string? importedBy { get; set; }
    public DateTime importedAt { get; set; }
    public int id { get; set; }
    public DateTime createdAt { get; set; }
    public object? updatedAt { get; set; }
}

public class MerchantLine
{
    public int batchId { get; set; }
    public int importId { get; set; }
    public string? sourceType { get; set; }
    public string? merchantSource { get; set; }
    public string? orderId { get; set; }
    public string? storeId { get; set; }
    public double amount { get; set; }
    public string? tenderType { get; set; }
    public string? authCode { get; set; }
    public DateTime transactionDate { get; set; }
    public DateTime BusinessDay { get; set; }
    public object? varianceAmount { get; set; }
    public string? importedBy { get; set; }
    public DateTime importedAt { get; set; }
    public bool flgDelete { get; set; }
    public bool flgActive { get; set; }
    public int id { get; set; }
    public DateTime createdAt { get; set; }
    public object? updatedAt { get; set; }
}

public class ReconciliationResult
{
    public bool success { get; set; }
    public string message { get; set; }
    public ReconciliationDTO data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}


public class ReconciliationDTO
{
    public int batchId { get; set; }
    public List<object> matchedCount { get; set; }
    public string status { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
}
// Reconciliation Result 
public class ReconciliationResultsDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public ReconciliationData Data { get; set; }
    public int StatusCode { get; set; }
    public object Errors { get; set; }
}

public class ReconciliationData
{
    public int BatchId { get; set; }
    public Batch batch { get; set; }
    public Payment payment { get; set; }
    // public List<ReconciliationItem> MatchedCount { get; set; }
     public MatchedCount matchedCount { get; set; }
       
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class MatchedCount
{
    public int headerId { get; set; }
    public List<ReconciliationItem> results { get; set; }
}
public class ReconciliationItem
{
    public string OrderId { get; set; }
    public string StoreId { get; set; }
    public string authCode { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public int Id { get; set; }
    public int BatchId { get; set; }
    public object Batch { get; set; }
    public bool isLocked { get; set; }
    public int MatchStep { get; set; }
    public string ReconStatus { get; set; }
    public string ExceptionType { get; set; }
    public decimal VarianceAmount { get; set; }
    public DateTime? MatchedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int InvoiceTransactionId { get; set; }
    public int MerchantTransactionId { get; set; }
    public string TenderType { get; set; }
    public DateTime BusinessDay { get; set; }
    public string MatchedBy { get; set; }
    public DateTime InvoiceBusinessDay { get; set; }
    public DateTime MerchantBusinessDay { get; set; }
    public decimal InvoiceAmountOriginal { get; set; }
    public decimal PaymentAmountOriginal { get; set; }
}

public class DeleteReconciliationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int ExceptionsDeleted { get; set; }
    public int ResultsDeleted { get; set; }
}