using Server.Models;
using System.Threading.Tasks;

namespace Server.UseCases.Interfaces
{
    public interface IMessageBroadcaster
    {
        Task BroadcastMessage(ChatEntry chatEntry);

        Task BroadcastChatRoomActivity(Participant participant, ChatRoomActivity chatRoomActivity);
    }
}
