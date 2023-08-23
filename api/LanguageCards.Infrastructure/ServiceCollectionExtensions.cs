using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDb.Options;
using MongoDb.Repository;

namespace LanguageCards.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));
            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value);

            return services;
        }

        public static IServiceCollection AddMongoDbRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(MongoDbRepository<>));

            return services;
        }
    }
}
