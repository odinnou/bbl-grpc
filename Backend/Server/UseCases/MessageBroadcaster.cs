using AutoMapper;
using Server.Models;
using Server.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace Server.UseCases
{
    public class MessageBroadcaster : IMessageBroadcaster
    {
        private readonly IChatRoomManager iChatRoomManager;
        private readonly IMapper iMapper;

        public MessageBroadcaster(IChatRoomManager iChatRoomManager, IMapper iMapper)
        {
            this.iChatRoomManager = iChatRoomManager ?? throw new ArgumentNullException(nameof(iChatRoomManager));
            this.iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }

        public async Task BroadcastChatRoomActivity(Participant participant, ChatRoomActivity chatRoomActivity)
        {
            MessageResponse messageResponse = iMapper.Map<MessageResponse>((participant, chatRoomActivity));

            await BroadcastResponse(messageResponse);
        }

        public async Task BroadcastMessage(ChatEntry chatEntry)
        {
            MessageResponse messageResponse = iMapper.Map<MessageResponse>(chatEntry);

            await BroadcastResponse(messageResponse);
        }

        private async Task BroadcastResponse(MessageResponse messageResponse)
        {
            foreach (Participant participant in iChatRoomManager.FetchParticipants())
            {
                await participant.ResponseStream.WriteAsync(messageResponse);
            }
        }
    }
}
