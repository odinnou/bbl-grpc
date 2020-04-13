using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Infrastructure
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
              : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ChatEntry> Entries { get; set; }
    }
}
