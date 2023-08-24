using LanguageCards.API.ApiModels.Auth;
using LanguageCards.API.Options;
using LanguageCards.Application.Services.Abstractions;
using LanguageCards.Domain.Entities.Auth;
using Microsoft.IdentityModel.Tokens;
using MongoDb.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LanguageCards.API.Services
{
    public class JwtAuthenticationManager : IAuthenticationManager
    {
        private readonly IRepository<UserCredentials> _userCredentialsRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly JwtOptions _jwtOptions;

        public JwtAuthenticationManager(
            IRepository<UserCredentials> userCredentialsRepository,
            IPasswordHashService passwordHashService,
            JwtOptions jwtOptions)
        {
            _userCredentialsRepository = userCredentialsRepository;
            _passwordHashService = passwordHashService;
            _jwtOptions = jwtOptions;
        }

        public async Task<AuthenticationResponse> GenerateNewTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecurityTokenHandler.ReadToken(refreshToken) as JwtSecurityToken;

            var userId = Guid.Parse(token.Subject);

            var user = await _userCredentialsRepository.FindByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                // todo: add exception
                return null;
            }

            // todo: add token type check

            var accessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken(user);

            return new AuthenticationResponse(accessToken, newRefreshToken);
        }

        public async Task<AuthenticationResponse> GenerateTokenAsync(AuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(authenticationRequest.Email) || string.IsNullOrEmpty(authenticationRequest.Password))
            {
                // todo: add exception
                return null;
            }

            var user = await _userCredentialsRepository.FindOneAsync(x => x.Email == authenticationRequest.Email, cancellationToken);

            if (user is null)
            {
                // todo: add exception
                return null;
            }

            if (!_passwordHashService.IsValidPassword(authenticationRequest.Password, user.Password, user.Salt))
            {
                return null;
            }

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user);

            return new AuthenticationResponse(accessToken, refreshToken);
        }

        private string GenerateAccessToken(UserCredentials user)
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

        private string GenerateRefreshToken(UserCredentials user)
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
