using AutoMapper;
using Client.Models;
using Client.Repositories.Interfaces;
using Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Repositories
{
    public class ServerApiConsumer : IServerApiConsumer
    {
        private readonly IMapper iMapper;
        private readonly Chat.ChatClient chatClient;

        public ServerApiConsumer(IMapper iMapper, Chat.ChatClient chatClient)
        {
            this.iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            this.chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
        }

        public async Task<IEnumerable<ChatEntry>> GetHistory(int lastMessages)
        {
            GetMessagesRequest request = iMapper.Map<GetMessagesRequest>(lastMessages);

            GetMessagesResponse response = await chatClient.GetHistoryAsync(request);

            return iMapper.Map<IEnumerable<ChatEntry>>(response);
        }
    }
}
