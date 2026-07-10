using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Reconciliation.Blazor;

public class JwtParser
{
    public static DateTime GetExpiry(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.ValidTo;
    }

    public static IEnumerable<Claim> GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token).Claims;
    }
}
