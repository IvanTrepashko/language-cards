using LanguageCards.API.Options;
using LanguageCards.API.Services;
using LanguageCards.API.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace LanguageCards.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value);

            services.Configure<PasswordHashOptions>(configuration.GetSection(nameof(PasswordHashOptions)));
            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<PasswordHashOptions>>().Value);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationManager, JwtAuthenticationManager>();
            services.AddTransient<IPasswordHashService, PasswordHashService>();

            return services;
        }
    }
}
