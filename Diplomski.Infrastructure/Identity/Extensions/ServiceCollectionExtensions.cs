using Diplomski.Application.Interfaces.ThirdPartyContracts;
using Diplomski.Infrastructure.Identity.Models;
using Diplomski.Infrastructure.Identity.Services;
using Diplomski.Infrastructure.Identity.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Diplomski.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Diplomski.Infrastructure.Contexts;

namespace Diplomski.Infrastructure.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            //Samo da vidimo da li smo u modu za development ili production
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //Samo da izvadimo iz appsettings.json podatke kao sto je connectionString 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            //Dodajemo bazu to jest DbContext za IdentityUser-a-----------------------------------------------------------
            services.AddDbContext<IdentityAppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("Diplomski.Infrastructure"));
            });

            //Dodajemo Identity-------------------------------------------------------------
            services.AddIdentity<IdentityAppUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityAppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();

            //Dodajemo Autentifikaciju preko Tokena pomocu JWT biblioteke-----------------------------------------------------------
            services.AddSingleton(typeof(JwtSecurityTokenHandler));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));

            services.Configure<JwtOption>(option =>
            {
                option.Audience = configuration["JWT:Audience"];
                option.Issuer = configuration["JWT:Issuer"];
                option.SigningKey = configuration["JWT:SigningKey"];
                option.SigninCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(config =>
            {
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["JWT:Audience"],
                    ValidIssuer = configuration["JWT:Issuer"],
                    IssuerSigningKey = signingKey,

                };
            });

            return services;
        }
    }
}
