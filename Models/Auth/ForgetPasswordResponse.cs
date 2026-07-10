namespace Reconciliation.Blazor;

public class ForgetPasswordResponse
{
   
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string data { get; set; }
    public string? statusCode { get; set; }
    public string? RefreshToken { get; set; }
}
