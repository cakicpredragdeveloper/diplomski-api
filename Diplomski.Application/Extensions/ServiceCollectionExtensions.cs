using Diplomski.Application.Interfaces;
using Diplomski.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diplomski.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ITrackService, TrackService>();

            return services;
        }
    }
}
