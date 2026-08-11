namespace Reconciliation.Blazor;

public class StoreUpdateResponse
{
    public bool success { get; set; }
    public string? message { get; set; }
    public string? data { get; set; }
    public int statusCode { get; set; }
    public object? errors { get; set; }
}
