using Microsoft.Extensions.DependencyInjection;
using GrpcConsumer.API;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using static Market.API.AdminProducer;

namespace Client.Configuration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient<IMarketApiConsumer, MarketApiConsumer>();
            #endregion

            return services;
        }
    }
}
