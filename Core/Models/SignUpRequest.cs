using System.ComponentModel.DataAnnotations;

namespace Reconciliation.Blazor.Core.Models
{
    public class SignUpRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}