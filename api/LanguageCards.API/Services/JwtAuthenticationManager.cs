using LanguageCards.API.ApiModels.Auth;
using LanguageCards.API.Options;
using LanguageCards.API.Services.Abstractions;
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
        private readonly IRepository<UserCredentials> _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly JwtOptions _jwtOptions;

        public JwtAuthenticationManager(
            IRepository<UserCredentials> userRepository,
            IPasswordHashService passwordHashService,
            JwtOptions jwtOptions)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _jwtOptions = jwtOptions;
        }

        public async Task<AuthenticationResponse> GenerateTokenAsync(AuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(authenticationRequest.Email) || string.IsNullOrEmpty(authenticationRequest.Password))
            {
                // todo: add exception
                return null;
            }

            var user = await _userRepository.FindOneAsync(x => x.Email == authenticationRequest.Email, cancellationToken);

            if (user is null)
            {
                // todo: add exception
                return null;
            }

            if (!_passwordHashService.IsValidPassword(authenticationRequest.Password, user.Password, user.Salt))
            {
                return null;
            }

            var tokenExpiry = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtTokenLifetime);
            var key = Encoding.ASCII.GetBytes(_jwtOptions.JwtSecurityKey);

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
                SigningCredentials = credentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            var accessToken = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse(accessToken, null);
        }
    }
}
