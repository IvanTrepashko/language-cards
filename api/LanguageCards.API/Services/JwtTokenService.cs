using LanguageCards.API.Options;
using LanguageCards.API.Services.Abstractions;
using LanguageCards.Domain.Entities.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LanguageCards.API.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public string GenerateAccessToken(UserCredentials user)
        {
            var tokenExpiry = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtTokenLifetime);
            var key = Encoding.ASCII.GetBytes(_jwtOptions.AccessTokenSecurityKey);

            var claims = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            });

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = tokenExpiry,
                SigningCredentials = credentials,
                TokenType = "access",
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            var accessToken = jwtSecurityTokenHandler.WriteToken(securityToken);
            return accessToken;
        }

        public string GenerateRefreshToken(UserCredentials user)
        {
            var tokenExpiry = DateTime.UtcNow.AddDays(7);

            var key = Encoding.ASCII.GetBytes(_jwtOptions.RefreshTokenSecurityKey);

            var claims = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            });

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = tokenExpiry,
                SigningCredentials = credentials,
                TokenType = "refresh",
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            var refreshToken = jwtSecurityTokenHandler.WriteToken(securityToken);
            return refreshToken;
        }
    }
}
