using Reconciliation.Blazor.Core.Models;

namespace Reconciliation.Blazor.Core.Interfaces
{
    public interface IAuthApi
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> SignUpAsync(SignUpRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    }
}