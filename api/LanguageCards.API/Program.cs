using LanguageCards.API.Extensions;
using LanguageCards.Application;
using LanguageCards.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var Key = Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:AccessTokenSecurityKey"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    ValidAudience = builder.Configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            // Infrastructure dependencies
            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddMongoDbRepository();

            // API dependencies
            builder.Services.AddApiOptions(builder.Configuration);
            builder.Services.AddApiServices();

            // Application dependencies
            builder.Services.AddApplicationOptions(builder.Configuration);
            builder.Services.AddApplicationServices();

            builder.Services.AddMediatR(cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(typeof(Application.ServiceCollectionExtensions).Assembly);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
