using System.ComponentModel.DataAnnotations;

namespace Reconciliation.Blazor;

public class PaymentData
{
    public int id { get; set; }
    [Required(ErrorMessage = "Payment name is required.")]
    [StringLength(100, ErrorMessage = "Payment name cannot exceed 100 characters.")]
    public string name { get; set; }
}


public class PaymentListDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<PaymentData> data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}