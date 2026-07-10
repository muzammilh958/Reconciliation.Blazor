using System.ComponentModel.DataAnnotations;
using static Reconciliation.Blazor.Core.Endpoints.ApiEndpoints;

namespace Reconciliation.Blazor;



public class StoreDTO
{
    public int id { get; set; }
    [Required(ErrorMessage = "Store name is required.")]
    [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
    public string name { get; set; }
}

public class StoreListDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<StoreDTO> data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}