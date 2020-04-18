using Server.Models;
using System.Threading.Tasks;

namespace Server.UseCases.Interfaces
{
    public  interface IMessageBroadcaster
    {
        Task Execute(ChatEntry chatEntry);
    }
}
