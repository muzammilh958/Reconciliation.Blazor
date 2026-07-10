namespace Reconciliation.Blazor;

public class InvoiceCreateResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public InvoiceType data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}
