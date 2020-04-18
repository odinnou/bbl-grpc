using Server.Models;
using Server.UseCases.Interfaces;
using System.Collections.Generic;

namespace Server.UseCases
{
    public class ChatRoomManager : IChatRoomManager
    {
        public void AddParticipantToTheRoom(Participant participant)
        {
            ChatRoom.CurrentRoom.AddParticipant(participant);
        }

        public void RemoveParticipantFromTheRoom(Participant participant)
        {
            ChatRoom.CurrentRoom.RemoveParticipant(participant);
        }

        public IEnumerable<Participant> FetchParticipants()
        {
            return ChatRoom.CurrentRoom.Participants;
        }
    }
}
