using Grpc.Core;
using System;

namespace Server.Models
{
    public class Participant
    {
        public string ConnectionId { get; set; }
        public string Login { get; set; }
        public IAsyncStreamWriter<MessageResponse> ResponseStream { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Participant participant && ConnectionId == participant.ConnectionId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConnectionId);
        }
    }
}
