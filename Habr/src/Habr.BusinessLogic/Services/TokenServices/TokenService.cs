using Habr.DataAccess.Entities;
using Habr.Security.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Habr.BusinessLogic.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenBuilder _tokenBuilder;

        public TokenService(
            IConfiguration config,
            ITokenBuilder tokenBuilder)
        {
            _configuration = config;
            _tokenBuilder = tokenBuilder;
        }

        public string CreateToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var expires = int.Parse(_configuration["Jwt:ExpiresInMinutes"]);

            var claims = SetClaims(user);

            var accessToken = _tokenBuilder.BuildAccessToken(issuer, audience, key, claims, expires);
            return accessToken;
        }

        private List<Claim> SetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.Name)
            };

            return claims;
        }
    }
}
