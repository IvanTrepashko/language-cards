using LanguageCards.Application.Options;
using LanguageCards.Application.Services;
using LanguageCards.Application.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LanguageCards.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PasswordHashOptions>(configuration.GetSection(nameof(PasswordHashOptions)));
            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<PasswordHashOptions>>().Value);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHashService, PasswordHashService>();

            return services;
        }
    }
}
