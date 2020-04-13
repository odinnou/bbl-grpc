using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 80, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });

                    options.Listen(IPAddress.Any, 81, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                })
                .UseStartup<Startup>();
            });
    }
}