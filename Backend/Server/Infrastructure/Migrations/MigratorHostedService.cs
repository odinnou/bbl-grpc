using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Infrastructure.Migrations
{
    public class MigratorHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MigratorHostedService(IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
        {
            this.serviceProvider = serviceProvider;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            ChatContext chatContext = scope.ServiceProvider.GetRequiredService<ChatContext>();

            await chatContext.Database.MigrateAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
