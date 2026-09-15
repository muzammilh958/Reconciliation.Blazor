using Reconciliation.Blazor.Models.Batch;

namespace Reconciliation.Blazor;

public class ManualAdjustmentDTO
{
    // public ManualAdjustmentDTO()
    // {
    //     invoiceLines = new List<InvoiceLine>();
    //     merchantLines = new List<MerchantLine>();
    // }
    public bool Success { get; set; }
    public string Message { get; set; } ="";
    // public List<InvoiceLine> invoiceLines { get; set; }
    // public List<MerchantLine> merchantLines { get; set; }

     public ManualAdjustmentData Data { get; set; } = new ();
    public int StatusCode { get; set; }
    public object errors { get; set; } =new();
}


public class ManualAdjustmentData
{
    public BatchDataDTO batch { get; set; } = new ();
    public List<InvoiceLine> InvoiceLines { get; set; } = [];
    public List<MerchantLine> MerchantLines { get; set; } = [];
    public int invoiceTotalCount { get; set; }
    public int merchantTotalCount { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }

}


public class ManualReconciliationRequest
{
    public int ReconciliationId { get; set; }

    public int BatchId { get; set; }

    public int PaymentId { get; set; }

    public string userId { get; set; } = "";

    public List<ManualReconciliationGroup> Groups { get; set; }
        = new();
}

public class ManualReconciliationGroup
{
    public List<int> MerchantTransactionIds { get; set; }
        = new();

    public List<int> InvoiceTransactionIds { get; set; }
        = new();
}

public class ManualReconciliationResponse
{
    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public object? Data { get; set; }
}