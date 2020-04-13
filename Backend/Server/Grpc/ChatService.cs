using AutoMapper;
using Grpc.Core;
using Server.Models;
using Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Grpc
{
    public class ChatService : Chat.ChatBase
    {
        private readonly IChatEntryRepository iChatEntryRepository;
        private readonly IMapper iMapper;

        public ChatService(IChatEntryRepository iChatEntryRepository, IMapper iMapper)
        {
            this.iChatEntryRepository = iChatEntryRepository;
            this.iMapper = iMapper;
        }

        public override async Task<GetMessagesResponse> GetHistory(GetMessagesRequest request, ServerCallContext context)
        {
            int take = 50;
            IEnumerable<ChatEntry> entries = await iChatEntryRepository.GetLastEntries(take);

            context.Status = new Status(StatusCode.OK, "Admin found for email/password couple.");

            return iMapper.Map<GetMessagesResponse>(entries);
        }

        public override async Task Participate(IAsyncStreamReader<PostMessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            return base.Participate(requestStream, responseStream, context);
        }
    }
}
