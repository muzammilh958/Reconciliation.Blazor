using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reconciliation.Blazor.Models.Auth
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class SignUpRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        // public UserDto? User { get; set; }
        public AuthData? Data { get; set; }

        public int StatusCode { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
    public class AuthData
    {
        public string accessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public UserDto? User { get; set; }
    }
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }


    public class Data
    {
        public string accessToken { get; set; } = string.Empty;
        public User? user { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public Data? data { get; set; }
        public int statusCode { get; set; }
        public object? errors { get; set; }
    }

    public class User
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string normalizedUserName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string normalizedEmail { get; set; } = string.Empty;
        public bool emailConfirmed { get; set; }
        public string passwordHash { get; set; } = string.Empty;
        public string securityStamp { get; set; } = string.Empty;
        public string concurrencyStamp { get; set; } = string.Empty;
        public object? phoneNumber { get; set; }
        public bool phoneNumberConfirmed { get; set; }
        public bool twoFactorEnabled { get; set; }
        public object? lockoutEnd { get; set; }
        public bool lockoutEnabled { get; set; }
        public int accessFailedCount { get; set; }
    }

    public class ValidateTokenResponse
    {
        public bool valid { get; set; }
        public string? userId { get; set; }
        public string? email { get; set; }
        public DateTime? expiresAt { get; set; }
        public bool? shouldRefresh { get; set; }
        public string? message { get; set; }
        public bool? expired { get; set; }
    }
}
