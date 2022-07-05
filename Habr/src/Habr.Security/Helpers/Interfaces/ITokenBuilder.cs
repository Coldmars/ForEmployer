using System.Security.Claims;

namespace Habr.Security.Helpers.Interfaces
{
    public interface ITokenBuilder
    {
        string BuildAccessToken(string issuer,
                                string audience,
                                string key,
                                IEnumerable<Claim> claims,
                                int expires);
    }
}
