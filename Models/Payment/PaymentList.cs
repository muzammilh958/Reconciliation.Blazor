using System.ComponentModel.DataAnnotations;

namespace Reconciliation.Blazor;

public class PaymentData
{
    public int id { get; set; }
    [Required(ErrorMessage = "Payment name column name is required.")]
    [StringLength(100, ErrorMessage = "Payment name cannot exceed 100 characters.")]
    public string? name { get; set; }

    [Required(ErrorMessage = "Order ID column name is required.")]
    [StringLength(100, ErrorMessage = "Order ID column name cannot exceed 100 characters.")]
    public string? OrderId { get; set; }

    [Required(ErrorMessage = "Transaction Date column name is required.")]
    [StringLength(100, ErrorMessage = "Transaction Date column name cannot exceed 100 characters.")]
    public string TransactionDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Store ID column name is required.")]
    [StringLength(100, ErrorMessage = "Store ID column name cannot exceed 100 characters.")]
    public string StoreId { get; set; } = string.Empty;

    

    [Required(ErrorMessage = "Tender column name is required.")]
    [StringLength(100, ErrorMessage = "Tender column name cannot exceed 100 characters.")]
    public string Tender { get; set; } = string.Empty;
    [Required(ErrorMessage = "Transaction Amount column name is required.")]
    [StringLength(100, ErrorMessage = "Transaction Amount column name cannot exceed 100 characters.")]
    public string TransactionAmount { get; set; } = string.Empty;
    [Required(ErrorMessage = "Authorization Code column name is required.")]
    [StringLength(100, ErrorMessage = "Authorization Code column name cannot exceed 100 characters.")]
    public string AuthorizationCode { get; set; } = string.Empty;
    [Required(ErrorMessage = "Merchant ID column name is required.")]
    [StringLength(100, ErrorMessage = "Merchant ID column name cannot exceed 100 characters.")]
    public string MerchantID { get; set; } = string.Empty;

    public bool IsAuthEnable { get; set; }=false;
    
}


public class PaymentListDTO
{
    public bool success { get; set; }
    public string? message { get; set; }
    public List<PaymentData>? data { get; set; }
    public int statusCode { get; set; }
    public object? errors { get; set; }
}