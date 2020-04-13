using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Configuration;
using Server.Infrastructure.Migrations;
using System.Reflection;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddCors();

            services.AddAutoMapper(Assembly.Load(typeof(Startup).Assembly.GetName().Name!));
            services.AddDependencies();
            services.AddHostedService<MigratorHostedService>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
              //  endpoints.MapGrpcService<ChatService>();
            });
        }
    }
}
