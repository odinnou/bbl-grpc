using Microsoft.Extensions.DependencyInjection;
using System;

namespace Client.Configuration
{
    public static class ThirdPartyConfiguration
    {
        public static IServiceCollection AddThirdParties(this IServiceCollection services)
        {
            // Autorise l'accès à des ressources gRPC en HTTP.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            #region gRPC
            services.AddGrpcClient<Server.Chat.ChatClient>(client => { client.Address = new Uri("http://server:81"); });
            #endregion

            return services;
        }
    }
}
