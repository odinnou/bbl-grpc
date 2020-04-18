using Microsoft.EntityFrameworkCore;
using Server.Infrastructure;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public class ChatEntryRepository : IChatEntryRepository
    {
        private readonly ChatContext chatContext;

        public ChatEntryRepository(ChatContext chatContext) => this.chatContext = chatContext;

        public async Task<IEnumerable<ChatEntry>> GetLastEntries(int take)
        {
            return await chatContext.Entries
                                    .OrderByDescending(entry => entry.DateCreated)
                                    .Take(take)
                                    .ToListAsync();
        }

        public async Task AddEntry(ChatEntry entry)
        {
            chatContext.Add(entry);
            await chatContext.SaveChangesAsync();
        }
    }
}
