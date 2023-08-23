namespace LanguageCards.API.ApiModels.Auth
{
    public record RegistrationRequest(string Username, string Password, string FirstName, string LastName, string Email);
}
