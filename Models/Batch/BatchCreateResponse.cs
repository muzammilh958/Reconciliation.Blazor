using Reconciliation.Blazor.Models.Batch;

namespace Reconciliation.Blazor;

public class BatchCreateResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public BatchDataDTO data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}

