namespace LanguageCards.API.Services.Abstractions
{
    public interface IPasswordHashService
    {
        string GetHash(string password, out byte[] salt);

        bool IsValidPassword(string password, string hash, byte[] salt);
    }
}
