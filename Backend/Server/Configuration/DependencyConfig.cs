using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Infrastructure;
using Server.Repositories;
using Server.UseCases;
using Server.UseCases.Interfaces;

namespace Server.Configuration
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            #region Database
            services.AddDbContext<ChatContext>(options => options.UseLazyLoadingProxies().UseNpgsql("Host=sqldata;Database=chat;User Id=postgres;Password=docker;").UseSnakeCaseNamingConvention());
            #endregion

            #region Repositories
            services.AddTransient<IMessageBroadcaster, MessageBroadcaster>();
            services.AddTransient<IChatRoomManager, ChatRoomManager>();
            #endregion

            #region Repositories
            services.AddTransient<IChatEntryRepository, ChatEntryRepository>();
            #endregion

            return services;
        }
    }
}
