using LanguageCards.API.Extensions;
using LanguageCards.Infrastructure;

namespace LanguageCards.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Infrastructure dependencies
            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddMongoDbRepository();

            // API dependencies
            builder.Services.AddOptions(builder.Configuration);
            builder.Services.AddServices();

            builder.Services.AddMediatR(cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(typeof(Application.ServiceCollectionExtensions).Assembly);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
