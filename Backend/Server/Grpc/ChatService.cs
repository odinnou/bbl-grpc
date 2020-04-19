using AutoMapper;
using Grpc.Core;
using Server.Models;
using Server.Repositories;
using Server.UseCases.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Grpc
{
    public class ChatService : Chat.ChatBase
    {
        private readonly IChatEntryRepository iChatEntryRepository;
        private readonly IMessageBroadcaster iMessageBroadcaster;
        private readonly IChatRoomManager iChatRoomManager;
        private readonly IMapper iMapper;

        public ChatService(IChatEntryRepository iChatEntryRepository, IMessageBroadcaster iMessageBroadcaster, IChatRoomManager iChatRoomManager, IMapper iMapper)
        {
            this.iChatEntryRepository = iChatEntryRepository;
            this.iMessageBroadcaster = iMessageBroadcaster;
            this.iChatRoomManager = iChatRoomManager;
            this.iMapper = iMapper;
        }

        public override async Task<GetMessagesResponse> GetHistory(GetMessagesRequest request, ServerCallContext context)
        {
            IEnumerable<ChatEntry> entries = await iChatEntryRepository.GetLastEntries(request.LastMessages);

            context.Status = new Status(StatusCode.OK, $"Last {request.LastMessages} chat entries...");

            return iMapper.Map<GetMessagesResponse>(entries);
        }

        public override async Task Participate(IAsyncStreamReader<PostMessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext())
            {
                return;
            }

            string connectionId = context.GetHttpContext().Connection.Id;
            Participant participant = iMapper.Map<Participant>((requestStream.Current, connectionId, responseStream));

            await Connect(participant);

            try
            {
                while (await requestStream.MoveNext())
                {
                    ChatEntry chatEntry = iMapper.Map<ChatEntry>(requestStream.Current);

                    await SendMessage(chatEntry);
                }
            }
            catch
            {
                await Disconnect(participant);
            }
        }

        private async Task Connect(Participant participant)
        {
            iChatRoomManager.AddParticipantToTheRoom(participant);
            await iMessageBroadcaster.BroadcastChatRoomActivity(participant, ChatRoomActivity.Join);
        }

        private async Task Disconnect(Participant participant)
        {
            iChatRoomManager.RemoveParticipantFromTheRoom(participant);
            await iMessageBroadcaster.BroadcastChatRoomActivity(participant, ChatRoomActivity.Leave);
        }

        private async Task SendMessage(ChatEntry chatEntry)
        {
            await iChatEntryRepository.AddEntry(chatEntry);
            await iMessageBroadcaster.BroadcastMessage(chatEntry);
        }
    }
}
