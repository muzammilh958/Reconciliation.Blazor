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
        public string name { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public bool isDeleted { get; set; }
        public bool is_locked { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
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
        public string reconStatus { get; set; }
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
        public string sourceType { get; set; }
        public string invoiceSource { get; set; }
        public string orderId { get; set; }
        public string storeId { get; set; }
        public double amount { get; set; }
        public string tenderType { get; set; }
        public string authCode { get; set; }
        public DateTime transactionDate { get; set; }
        public DateTime businessDay { get; set; }
        public double varianceAmount { get; set; }
        public bool flgActive { get; set; }
        public bool flgDelete { get; set; }
        public string importedBy { get; set; }
        public DateTime importedAt { get; set; }
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public object updatedAt { get; set; }
    }

    public class MerchantLine
    {
        public int batchId { get; set; }
        public int importId { get; set; }
        public string sourceType { get; set; }
        public string merchantSource { get; set; }
        public string orderId { get; set; }
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
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public object updatedAt { get; set; }
    }

    public class ReconciliationResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public int statusCode { get; set; }
        public object errors { get; set; }
    }
