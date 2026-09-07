namespace Reconciliation.Blazor;

public class RecReconciliationExceptionDTOData
{
    public int batchId { get; set; }
    public string exceptionType { get; set; }
    public int reconciliationId { get; set; }
    public DateTime createdAt { get; set; }
    public int id { get; set; }
    public object? updatedAt { get; set; }
    public int? invoiceLineId { get; set; }
    public object? merchantLineId { get; set; }
    public string? exceptionDescription { get; set; }
}

public class RecReconciliationExceptionPagedData
{
    public List<RecReconciliationExceptionDTOData> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class RecReconciliationExceptionDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public RecReconciliationExceptionPagedData data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}