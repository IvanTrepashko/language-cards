using LanguageCards.Domain.Abstractions;
using LanguageCards.Domain.ValueObject;

namespace LanguageCards.Domain.Entities.User
{
    public class User : BaseEntity
    {
        public override Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Email Email { get; set; }

        public string Username { get; set; }

        public User(Guid id, string firstName, string lastName, Email email, string username)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
        }
    }
}
