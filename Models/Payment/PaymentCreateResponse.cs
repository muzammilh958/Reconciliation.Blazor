namespace Reconciliation.Blazor;

public class PaymentCreateResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public PaymentData data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}
