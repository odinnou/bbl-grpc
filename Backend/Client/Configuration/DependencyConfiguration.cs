using Client.Repositories;
using Client.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Configuration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient<IServerApiConsumer, ServerApiConsumer>();
            #endregion

            return services;
        }
    }
}
