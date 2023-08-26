using LanguageCards.Domain.Entities.Auth;

namespace LanguageCards.API.Services.Abstractions
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(UserCredentials user);

        string GenerateRefreshToken(UserCredentials user);
    }
}
