namespace Reconciliation.Blazor;

public class MerchantCreateResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public Data data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}
public class Data
{
    public int id { get; set; }
    public string message { get; set; }
}
