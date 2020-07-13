using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;

namespace Client.Configuration
{
    public static class ThirdPartyConfiguration
    {
        public static IServiceCollection AddThirdParties(this IServiceCollection services)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandlers", false);
            }

            #region gRPC
            //services.AddGrpcClient < Server>(o =>
            //{
            //    o.Address = new Uri("");
            //});
            #endregion

            return services;
        }
    }
}
