using LanguageCards.API.Options;
using LanguageCards.API.Services.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace LanguageCards.API.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly PasswordHashOptions _passwordHashOptions;

        public PasswordHashService(PasswordHashOptions passwordHashOptions)
        {
            _passwordHashOptions = passwordHashOptions;
        }

        public string GetHash(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(_passwordHashOptions.KeySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _passwordHashOptions.Iterations,
                HashAlgorithmName.SHA512,
                _passwordHashOptions.KeySize);

            return Convert.ToHexString(hash);
        }

        public bool IsValidPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                _passwordHashOptions.Iterations,
                HashAlgorithmName.SHA512,
                _passwordHashOptions.KeySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
