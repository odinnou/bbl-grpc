using Microsoft.Extensions.DependencyInjection;

namespace Client.Configuration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            #region Repositories
            //services.AddTransient<IMarketApiConsumer, MarketApiConsumer>();
            #endregion

            return services;
        }
    }
}
