using Server.Models;
using System.Collections.Generic;

namespace Server.UseCases.Interfaces
{
    public interface IChatRoomManager
    {
        void AddParticipantToTheRoom(Participant participant);
        void RemoveParticipantFromTheRoom(Participant participant);
        IEnumerable<Participant> FetchParticipants();
    }
}
