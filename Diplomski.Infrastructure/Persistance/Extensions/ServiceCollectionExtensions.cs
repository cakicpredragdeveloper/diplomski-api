using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using Diplomski.Infrastructure.Persistance.Repositories;

namespace Diplomski.Infrastructure.Persistance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();
            var settings = new ConnectionSettings(new Uri(configuration["elasticsearch:url"]));

            services.AddSingleton<IElasticClient>(new ElasticClient(settings));
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ITrackRepository, TrackRepository>();

            return services;
        }
    }
}
