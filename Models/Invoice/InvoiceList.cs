using System.ComponentModel.DataAnnotations;
namespace Reconciliation.Blazor;

public class InvoiceType
{
    public int id { get; set; }
    [Required(ErrorMessage = "Invoice name is required.")]
    [StringLength(100, ErrorMessage = "Invoice name cannot exceed 100 characters.")]
    public string name { get; set; }

    
}
public class InvoiceTypeList
{

    public bool success { get; set; }
    public string message { get; set; }
    public List<InvoiceType> data { get; set; }= new();
    public int statusCode { get; set; }
    public object errors { get; set; }

}
