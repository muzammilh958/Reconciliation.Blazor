namespace Reconciliation.Blazor;

public class Exceptionlist
{
    public int id { get; set; }
    public int batchId { get; set; }
    public int? invoiceLineId { get; set; }
    public int? merchantLineId { get; set; }
    public string exceptionType { get; set; }
    public string exceptionDescription { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public object batch { get; set; }
    public object invoiceLine { get; set; }
    public object merchantLine { get; set; }
}

public class ExceptionlistDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<Exceptionlist> data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}

