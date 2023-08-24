namespace LanguageCards.API.ApiModels.Auth
{
    public record RegistrationRequest(string Email, string Password, string FirstName, string LastName);
}
