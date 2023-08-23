using System.Net.Mail;

namespace LanguageCards.Domain.ValueObject
{
    public class Email : ValueObject
    {
        public string Value { get; init; }

        public Email(string value)
        {
            if (MailAddress.TryCreate(value, out _))
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException("Invalid email format", nameof(value));
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
