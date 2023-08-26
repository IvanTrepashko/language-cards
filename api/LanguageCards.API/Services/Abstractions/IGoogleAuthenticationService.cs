using LanguageCards.API.ApiModels.Auth;

namespace LanguageCards.API.Services.Abstractions
{
    public interface IGoogleAuthenticationService
    {
        Task<AuthenticationResponse> HandleGoogleAuthenticationAsync(string googleAccessToken, CancellationToken cancellationToken);
    }
}
