using System.Collections.Generic;

namespace Server.Models
{
    /// <summary>
    /// Ne convient pas dans un environnement multi-instance
    /// </summary>
    public class ChatRoom
    {
        public static readonly ChatRoom CurrentRoom = new ChatRoom();

        public ChatRoom()
        {
            Participants = new List<Participant>();
        }

        public List<Participant> Participants { get; }

        public void AddParticipant(Participant participantToAdd)
        {
            if (!Participants.Contains(participantToAdd))
            {
                Participants.Add(participantToAdd);
            }
        }

        public void RemoveParticipant(Participant participantRoRemove)
        {
            Participants.Remove(participantRoRemove);
        }
    }

    public enum ChatRoomActivity
    {
        Join,
        Leave
    }
}
