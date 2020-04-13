using Microsoft.EntityFrameworkCore;
using Server.Infrastructure;
using Server.Models;
using Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public class ChatEntryRepository : IChatEntryRepository
    {
        private readonly ChatContext chatContext;

        protected ChatEntryRepository(ChatContext chatContext) => this.chatContext = chatContext;

        public async Task<IEnumerable<ChatEntry>> GetLastEntries(int take)
        {
            return await chatContext.Entries
                                    .OrderByDescending(entry => entry.DateCreated)
                                    .Take(take)
                                    .ToListAsync();
        }

        public async Task<ChatEntry?> GetOldestEntrySince(DateTime dateTime)
        {
            return await chatContext.Entries
                                    .Where(entry => entry.DateCreated > dateTime)
                                    .OrderBy(entry => entry.DateCreated)
                                    .FirstOrDefaultAsync();
        }

        public async Task<ChatEntry> AddEntry(ChatEntry entry)
        {
            chatContext.Add(entry);
            await chatContext.SaveChangesAsync();

            return entry;
        }
    }
}
