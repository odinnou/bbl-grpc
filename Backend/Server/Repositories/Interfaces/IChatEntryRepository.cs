using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public interface IChatEntryRepository
    {
        Task<IEnumerable<ChatEntry>> GetLastEntries(int take);
        Task AddEntry(ChatEntry entry);
    }
}
