using MicroZoo.Infrastructure.Models.Users;
using System.Security.Claims;

namespace MicroZoo.IdentityApi.JwtFeatures
{
    public interface IJwtHandler
    {
        string CreateAccessToken(User user, IList<string> roles);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetRefreshTokenExpiryTimeSpanInDays();
        ClaimsPrincipal GetPrincipalFromHttpRequest(HttpRequest request);
    }
}
