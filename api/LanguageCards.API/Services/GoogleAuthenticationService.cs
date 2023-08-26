using Google.Apis.Auth;
using Google.Apis.Util;
using LanguageCards.API.ApiModels.Auth;
using LanguageCards.API.Options;
using LanguageCards.API.Services.Abstractions;
using LanguageCards.Application.Commands.Authorization;
using LanguageCards.Domain.Entities.Auth;
using MediatR;
using MongoDb.Repository;

namespace LanguageCards.API.Services
{
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        private readonly GoogleAuthOptions _googleAuthOptions;
        private readonly ISender _sender;
        private readonly IRepository<UserCredentials> _userCredentialsRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public GoogleAuthenticationService(
            GoogleAuthOptions googleAuthOptions,
            ISender sender,
            IRepository<UserCredentials> userCredentialsRepository,
            IJwtTokenService jwtTokenService)
        {
            _googleAuthOptions = googleAuthOptions;
            _sender = sender;
            _userCredentialsRepository = userCredentialsRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthenticationResponse> HandleGoogleAuthenticationAsync(string googleAccessToken, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _googleAuthOptions.Audience },
                Clock = new Clock()
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(googleAccessToken, settings);

            var user = await _userCredentialsRepository.FindOneAsync(x => string.Equals(x.Email, payload.Email), cancellationToken);

            if (user is null)
            {
                var registerUserCommand = new RegisterUserCommand(payload.Email, null, payload.GivenName, payload.FamilyName);

                user = await _sender.Send(registerUserCommand, cancellationToken);
            }

            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(user);

            return new AuthenticationResponse(accessToken, refreshToken);
        }

        private class Clock : IClock
        {
            public DateTime Now => DateTime.Now.AddMinutes(6);

            public DateTime UtcNow => DateTime.UtcNow.AddMinutes(6);
        }
    }
}
