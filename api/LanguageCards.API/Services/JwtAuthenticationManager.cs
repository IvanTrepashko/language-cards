using LanguageCards.API.ApiModels.Auth;
using LanguageCards.API.Services.Abstractions;
using LanguageCards.Application.Services.Abstractions;
using LanguageCards.Domain.Entities.Auth;
using MongoDb.Repository;
using System.IdentityModel.Tokens.Jwt;

namespace LanguageCards.API.Services
{
    public class JwtAuthenticationManager : IAuthenticationManager
    {
        private readonly IRepository<UserCredentials> _userCredentialsRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJwtTokenService _jwtTokenService;

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

            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(user);

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

            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(user);

            return new AuthenticationResponse(accessToken, refreshToken);
        }
    }
}
