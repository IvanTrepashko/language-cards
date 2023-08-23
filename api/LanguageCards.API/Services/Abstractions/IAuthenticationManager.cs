using LanguageCards.API.ApiModels.Auth;

namespace LanguageCards.API.Services.Abstractions
{
    public interface IAuthenticationManager
    {
        Task<AuthenticationResponse> GenerateTokenAsync(AuthenticationRequest request, CancellationToken cancellationToken);
    }
}
