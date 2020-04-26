using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Infrastructure.Migrations
{
    public class MigratorHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public MigratorHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            ChatContext chatContext = scope.ServiceProvider.GetRequiredService<ChatContext>();

            await chatContext.Database.EnsureDeletedAsync();
            await chatContext.Database.MigrateAsync();

            if (!await chatContext.Entries.AnyAsync())
            {
                IEnumerable<ChatEntry> fakeChatEntries = BuildRandomChatEntries();
                chatContext.AddRange(fakeChatEntries);
                await chatContext.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private IEnumerable<ChatEntry> BuildRandomChatEntries()
        {
            string[] logins = new[] { "Odinnou", "Cgt", "Tazacban", "Dashell", "Cwep", "16ar" };

            Faker<ChatEntry> faker = new Faker<ChatEntry>()
                            .RuleFor(entry => entry.DateCreated, fake => fake.Date.Past())
                            .RuleFor(entry => entry.Message, fake => fake.Lorem.Sentences(2))
                            .RuleFor(entry => entry.Login, fake => fake.PickRandom(logins));

            return faker.Generate(200);
        }
    }
}
