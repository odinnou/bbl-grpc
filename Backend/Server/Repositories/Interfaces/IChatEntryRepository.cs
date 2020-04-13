using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repositories.Interfaces
{
    public interface IChatEntryRepository
    {
        Task<IEnumerable<ChatEntry>> GetLastEntries(int take);
        Task<ChatEntry?> GetOldestEntrySince(DateTime dateTime);
        Task<ChatEntry> AddEntry(ChatEntry entry);
    }
}
