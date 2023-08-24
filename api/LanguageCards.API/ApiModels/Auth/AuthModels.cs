namespace LanguageCards.API.ApiModels.Auth
{
    public record AuthenticationRequest(string Email, string Password);

    public record AuthenticationResponse(string AccessToken, string RefreshToken);

    public record RefreshTokenRequest(string RefreshToken);
}
