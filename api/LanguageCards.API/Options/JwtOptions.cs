namespace LanguageCards.API.Options
{
    public class JwtOptions
    {
        public string AccessTokenSecurityKey { get; set; }

        public int JwtTokenLifetime { get; set; }

        public string RefreshTokenSecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
