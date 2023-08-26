using LanguageCards.API.Options;
using LanguageCards.API.Services;
using LanguageCards.API.Services.Abstractions;
using LanguageCards.Application.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace LanguageCards.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value);

            services.Configure<GoogleAuthOptions>(configuration.GetSection(nameof(GoogleAuthOptions)));
            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<GoogleAuthOptions>>().Value);

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationManager, JwtAuthenticationManager>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IGoogleAuthenticationService, GoogleAuthenticationService>();

            return services;
        }
    }
}
