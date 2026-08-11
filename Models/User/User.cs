using System.Text.Json.Serialization;

namespace Reconciliation.Blazor;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? FirstName  { get; set; }

    public string? LastName  { get; set; }

    public bool IsLocked { get; set; }

    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; }

    
    public string? RoleName { get; set; }   
}
