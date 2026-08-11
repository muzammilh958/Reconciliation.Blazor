namespace Reconciliation.Blazor.Core.Models
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public LoginData? Data { get; set; }
        public int StatusCode { get; set; }
        public object? Errors { get; set; }
    }
     public class LoginData
    {
        public string? AccessToken { get; set; }
        public UserData? User { get; set; }
    }

    public class UserData
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public List<string>? Roles { get; set; } // Add this if you need roles
    }
}