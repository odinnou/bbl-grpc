using Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Repositories.Interfaces
{
    public interface IServerApiConsumer
    {
        Task<IEnumerable<ChatEntry>> GetHistory(int lastMessages);
    }
}
