namespace Reconciliation.Blazor;

public class InvoiceDeleteResponse
{
    public bool success { get; set; }
    public string? message { get; set; }
    public string? data { get; set; }
    public int statusCode { get; set; }
    public List<dynamic>? errors { get; set; }
}
