namespace LanguageCards.API.Options
{
    public class JwtOptions
    {
        public string JwtSecurityKey { get; set; }

        public int JwtTokenLifetime { get; set; }
    }
}
