using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reconciliation.Blazor.Models.Batch;

public class BatchDataDTO
{
    public int id { get; set; }
    [Required(ErrorMessage = "Batch name is required.")]
    [StringLength(100, ErrorMessage = "Batch name cannot exceed 100 characters.")]
    public string name { get; set; }
    [JsonPropertyName("is_Locked")]
    public bool IsLocked { get; set; }
    [Required(ErrorMessage = "From Date is required.")]
    public DateTime? FromDate { get; set; }
    [Required(ErrorMessage = "To Date is required.")]
    public DateTime? ToDate { get; set; }
    public string is_Locked_raw
    {
        set => IsLocked = value?.ToLower() == "true";
    }
  
}

public class BatchListDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<BatchDataDTO> data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }
}