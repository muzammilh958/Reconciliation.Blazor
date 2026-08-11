namespace Reconciliation.Blazor;

public class StoreCreateResponse
{
    public bool success { get; set; }
    public string? message { get; set; }
    public StoreDTO? data { get; set; }
    public int statusCode { get; set; }
    public object? errors { get; set; }
}
