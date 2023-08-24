using LanguageCards.API.ApiModels.Auth;

namespace LanguageCards.Application.Services.Abstractions
{
    public interface IAuthenticationManager
    {
        Task<AuthenticationResponse> GenerateTokenAsync(AuthenticationRequest request, CancellationToken cancellationToken);

        Task<AuthenticationResponse> GenerateNewTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
