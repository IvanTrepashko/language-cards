using LanguageCards.Domain.Abstractions;

namespace LanguageCards.Domain.Entities.Auth
{
    public class UserCredentials : BaseEntity
    {
        public override Guid Id { get; set; }

        public string Email { get; init; }

        public string Password { get; init; }

        public byte[] Salt { get; init; }

        public string Role { get; init; }

        public UserCredentials(Guid id, string email, string password, byte[] salt, string role)
        {
            Id = id;
            Email = email;
            Password = password;
            Salt = salt;
            Role = role;
        }
    }
}
