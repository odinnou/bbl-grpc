using AutoMapper;
using Grpc.Core;
using Server.Models;
using Server.Repositories;
using Server.UseCases;
using Server.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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

            var lol = iMapper.Map<GetMessagesResponse>(entries);
            return lol;
        }

        public override async Task Participate(IAsyncStreamReader<PostMessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext())
            {
                return;
            }

            string connectionId = context.GetHttpContext().Connection.Id;
            Participant participant = iMapper.Map<Participant>((requestStream.Current, connectionId, responseStream));

            iChatRoomManager.AddParticipantToTheRoom(participant);

            try
            {
                while (await requestStream.MoveNext())
                {
                    ChatEntry chatEntry = iMapper.Map<ChatEntry>(requestStream.Current);

                    await iChatEntryRepository.AddEntry(chatEntry);
                    await iMessageBroadcaster.Execute(chatEntry);
                }
            }
            catch (Exception)
            {
                iChatRoomManager.RemoveParticipantFromTheRoom(participant);
            }
        }
    }
}
