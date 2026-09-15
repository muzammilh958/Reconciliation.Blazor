using System.ComponentModel.DataAnnotations;
namespace Reconciliation.Blazor;

public class InvoiceType
{
    public int id { get; set; }
    [Required(ErrorMessage = "Invoice name is required.")]
    [StringLength(100, ErrorMessage = "Invoice name cannot exceed 100 characters.")]
    public string? name { get; set; }

    
}
public class InvoiceTypeList
{

    public bool success { get; set; }
    public string? message { get; set; }
    public List<InvoiceType> data { get; set; }= new();
    public int statusCode { get; set; }
    public object? errors { get; set; }

}


  public class FetchInvoiceTypeResponse
    {
        public string name { get; set; } = "";
        public bool flgActive { get; set; }
        public bool flgDelete { get; set; }
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class SingleInvoiceTypeResponse
    {
        public bool success { get; set; }
        public string message { get; set; } = "";
        public FetchInvoiceTypeResponse data { get; set; }
        public int statusCode { get; set; }
        public object errors { get; set; }
    }
