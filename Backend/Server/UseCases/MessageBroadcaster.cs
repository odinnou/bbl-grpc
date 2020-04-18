using AutoMapper;
using Server.Models;
using Server.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace Server.UseCases
{
    public class MessageBroadcaster: IMessageBroadcaster
    {
        private readonly IChatRoomManager iChatRoomManager;
        private readonly IMapper iMapper;

        public MessageBroadcaster(IChatRoomManager iChatRoomManager, IMapper iMapper)
        {
            this.iChatRoomManager = iChatRoomManager ?? throw new ArgumentNullException(nameof(iChatRoomManager));
            this.iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }

        public async Task Execute(ChatEntry chatEntry)
        {
            MessageResponse messageResponse = iMapper.Map<MessageResponse>(chatEntry);

            foreach (Participant participant in iChatRoomManager.FetchParticipants())
            {
                await participant.ResponseStream.WriteAsync(messageResponse);
            }
        }
    }
}
